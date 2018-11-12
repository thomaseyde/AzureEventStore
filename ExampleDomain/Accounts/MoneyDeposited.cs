namespace ExampleDomain.Accounts
{
	class MoneyDeposited
	{
		public string AccountNumber { get; }
		public decimal Amount { get; }

		public MoneyDeposited(string accountNumber, decimal amount)
		{
			AccountNumber = accountNumber;
			Amount = amount;
		}
	}
}