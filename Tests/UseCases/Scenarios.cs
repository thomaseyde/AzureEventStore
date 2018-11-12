using AzureEventStore.Testing;
using ExampleDomain;
using ExampleDomain.Accounts;
using ExampleDomain.Cards;
using ExampleDomain.Persistence;
using FluentAssertions;
using Xunit;

namespace Tests.UseCases
{
	public class Scenarios 
	{
		private readonly Repository repository;

		public Scenarios()
		{
			repository = new Repository("Account", new TypeResolver(), new MemoryProvider());
		}

		[Fact]
        public void Read_event_sourced_aggregate()
        {
	        repository.Save(OpenedAccountWithTransactions("1"));
            var account = repository.Get<Account>("1");

	        account.Balance.Should().Be(406000);
	        account.Number.Should().Be("1");
	        account.IsOpen.Should().BeTrue();
        }

        [Fact]
        public void Read_snapshot_aggregate()
        {
	        var zero = Creditcard.Issue("2");
	        
	        ApplyTransactions(zero, 6);

	        repository.Save(zero);

	        var first = repository.Get<Creditcard>("2");

	        ApplyTransactions(first, 3);

	        repository.Save(first);

	        var final = repository.Get<Creditcard>("2");


	        final.Balance.Should().Be(-52);
        }

        static void ApplyTransactions(Creditcard card, int count)
        {
	        for (var i = 1; i <= count; i++)
            {
                if (i % 5 == 0)
                {
                    card.PayIssuer(card.Balance);
                }
                else
                {
                    card.PostTransaction(i + 10);
                }
            }
        }

        static Account OpenedAccountWithTransactions(string accountNumber)
        {
            var account = Account.Open(accountNumber);

            for (var i = 0; i < 1000; i++)
            {
                if (i%5==0)
                {
                    account.Withdraw(10m);
                }
                else
                {
                    account.Deposit(i + 10);
                }
            }

            return account;
        }
    }
}