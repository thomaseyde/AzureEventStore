namespace ExampleDomain.Cards
{
	class CreditcardIssuerPaid
	{
		public string CreditcardNumber { get; }
		public decimal Amount { get; }

		public CreditcardIssuerPaid(string creditcardNumber, decimal amount)
		{
			CreditcardNumber = creditcardNumber;
			Amount = amount;
		}
	}
}