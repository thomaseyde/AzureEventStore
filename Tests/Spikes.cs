using System;
using System.Collections.Generic;
using FluentAssertions;
using Tests.RedesignApi.Domain;
using Tests.RedesignApi.Framework;
using Xunit;

namespace Tests
{
	namespace RedesignApi
	{
		public class Spikes : AggregateTest
		{
			[Fact]
			public void PoC()
			{
				var initial = new object[]
				              {
					              new AccountOpened("1"),
					              new MoneyDeposited("1", 1000m),
					              new MoneyWithdrawn("1", 100m)
				              };

				var accountAtVersion3 = CreateFromHistory<Account>(initial);

				accountAtVersion3.Version.Should().Be(3);
				accountAtVersion3.AccountNumber.Should().Be("1");
				accountAtVersion3.Balance.Should().Be(900m);

				var snapshot = new AccountSnapshot(10, "1", 500m);

				var accountAtVersion10 = CreateFromSnapshot<Account>(snapshot);

				accountAtVersion10.Version.Should().Be(10);
				accountAtVersion10.AccountNumber.Should().Be("1");
				accountAtVersion10.Balance.Should().Be(500m);

				var history = new object[]
				              {
					              new MoneyDeposited("1", 1000m),
					              new MoneyWithdrawn("1", 100m)
				              };

				var accountAtVersion12 = CreateFromSnapshotAndHistory<Account>(snapshot, history);

				accountAtVersion12.Version.Should().Be(12);
				accountAtVersion12.AccountNumber.Should().Be("1");
				accountAtVersion12.Balance.Should().Be(1400m);
			}
		}

		public class AggregateTest
		{
			protected static TAggregate CreateFromSnapshotAndHistory<TAggregate>(ISnapshot snapshot, IEnumerable<object> history) where TAggregate : ILoadAggregateState, new()
			{
				ILoadAggregateState account = new TAggregate();
				account.LoadFromSnapshot(snapshot);
				account.LoadFromHistory(history);
				return (TAggregate) account;
			}

			protected static TAggregate CreateFromSnapshot<TAggregate>(ISnapshot snapshot) where TAggregate : ILoadAggregateState, new()
			{
				ILoadAggregateState account = new TAggregate();
				account.LoadFromSnapshot(snapshot);
				return (TAggregate) account;
			}

			protected static TAggregate CreateFromHistory<TAggregate>(IEnumerable<object> history) where TAggregate : ILoadAggregateState, new()
			{
				ILoadAggregateState account = new TAggregate();
				account.LoadFromHistory(history);
				return (TAggregate) account;
			}
		}

		namespace Domain
		{
			class Account : Aggregate<AccountSnapshot>
			{
				public decimal Balance { get; private set; }
				public string AccountNumber { get; private set; }

				protected override void ApplySnapshot(AccountSnapshot snapshot)
				{
					AccountNumber = snapshot.AccountNumber;
					Balance = snapshot.Balance;
				}

				protected override void ApplyEvent(object e)
				{
					Match.Message(e)
					     .To<AccountOpened>(Apply)
					     .To<MoneyWithdrawn>(Apply)
					     .To<MoneyDeposited>(Apply);
				}

				private void Apply(MoneyWithdrawn withdrawn)
				{
					Balance -= withdrawn.Amount;
				}

				private void Apply(MoneyDeposited deposited)
				{
					Balance += deposited.Amount;
				}

				private void Apply(AccountOpened opened)
				{
					AccountNumber = opened.AccountNumber;
				}
			}

			class AccountSnapshot : ISnapshot
			{
				public AccountSnapshot(int version, string accountNumber, decimal balance)
				{
					AccountNumber = accountNumber;
					Version = version;
					Balance = balance;
				}

				public string AccountNumber { get; }
				public decimal Balance { get; }
				public int Version { get; }
			}

			class MoneyWithdrawn
			{
				public MoneyWithdrawn(string accountNumber, decimal amount)
				{
					AccountNumber = accountNumber;
					Amount = amount;
				}

				public string AccountNumber { get; }
				public decimal Amount { get; }
			}

			class MoneyDeposited
			{
				public MoneyDeposited(string accountNumber, decimal amount)
				{
					AccountNumber = accountNumber;
					Amount = amount;
				}

				public string AccountNumber { get; }
				public decimal Amount { get; }
			}

			class AccountOpened
			{
				public AccountOpened(string accountNumber)
				{
					AccountNumber = accountNumber;
				}

				public string AccountNumber { get; }
			}
		}

		namespace Framework
		{
			public interface ILoadAggregateState
			{
				void LoadFromHistory(IEnumerable<object> history);
				void LoadFromSnapshot(ISnapshot snapshot);
			}

			abstract class Aggregate<TSnapshot> : ILoadAggregateState where TSnapshot : ISnapshot
			{
				public int Version { get; private set; }

				public void LoadFromSnapshot(ISnapshot snapshot) => LoadFromSnapshot((TSnapshot) snapshot);

				public void LoadFromHistory(IEnumerable<object> history)
				{
					foreach (var e in history)
					{
						ApplyEvent(e);
						Version++;
					}
				}

				private void LoadFromSnapshot(TSnapshot snapshot)
				{
					ApplySnapshot(snapshot);
					Version = snapshot.Version;
				}

				protected abstract void ApplyEvent(object e);
				protected abstract void ApplySnapshot(TSnapshot snapshot);
			}

			public interface ISnapshot
			{
				int Version { get; }
			}

			class Match : IMatch
			{
				private readonly object source;

				private Match(object source)
				{
					this.source = source;
				}

				public IMatch To<T>(Action<T> applier)
				{
					if (!(source is T t)) return this;

					applier.Invoke(t);
					return new Matched();
				}

				public static Match Message(object o)
				{
					return new Match(o);
				}

				private class Matched : IMatch
				{
					public IMatch To<T>(Action<T> applier)
					{
						return this;
					}
				}
			}

			interface IMatch
			{
				IMatch To<T>(Action<T> applier);
			}
		}
	}
}