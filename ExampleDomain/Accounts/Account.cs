using Aggregates;

namespace ExampleDomain.Accounts
{
	public class Account : EventSourceAggregate
	{
		public static Account Open(string accountNumber)
		{
			var account = new Account();
			account.Raise(new AccountOpened(accountNumber));
			return account;
		}

		public void Withdraw(decimal amount)
		{
			Raise(new MoneyWithdrawn(Number, amount));
		}

		public void Deposit(decimal amount)
		{
			Raise(new MoneyDeposited(Number, amount));
		}

		protected override void ApplyEvent(object e)
		{
			switch (e)
			{
				case  AccountOpened opened:
					Apply(opened);
					break;
				case  MoneyDeposited deposited:
					Apply(deposited);
					break;
				case  MoneyWithdrawn withdrawn:
					Apply(withdrawn);
					break;
			}
		}

		void Apply(AccountOpened e)
		{
			IsOpen = true;
			Number = e.Number;
		}

		void Apply(MoneyDeposited e)
		{
			Balance += e.Amount;
		}
            
		void Apply(MoneyWithdrawn e)
		{
			Balance -= e.Amount;
		}

		public bool IsOpen { get; private set; }
		public string Number { get; private set; }
		public decimal Balance { get; private set; }
		public override string Id => Number;
	}
}