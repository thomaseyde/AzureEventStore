namespace ExampleDomain.Accounts
{
	class MoneyWithdrawn
	{
		public string AccountNumber { get; }
		public decimal Amount { get; }

		public MoneyWithdrawn(string accountNumber, decimal amount)
		{
			AccountNumber = accountNumber;
			Amount = amount;
		}
	}
}