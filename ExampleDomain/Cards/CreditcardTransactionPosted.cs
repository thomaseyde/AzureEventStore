namespace ExampleDomain.Cards
{
	class CreditcardTransactionPosted
	{
		public string CreditcardNumber { get; }
		public decimal Amount { get; }

		public CreditcardTransactionPosted(string creditcardNumber, decimal amount)
		{
			CreditcardNumber = creditcardNumber;
			Amount = amount;
		}
	}
}