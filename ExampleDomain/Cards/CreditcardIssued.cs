namespace ExampleDomain.Cards
{
	class CreditcardIssued
	{
		public string CreditcardNumber { get; }

		public CreditcardIssued(string creditcardNumber)
		{
			CreditcardNumber = creditcardNumber;
		}
	}
}