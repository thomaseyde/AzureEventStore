using Aggregates.Persistence;

namespace ExampleDomain.Cards
{
	public class CreditcardSnapshot : ISnapshot
	{
		public string Number { get; }
		public decimal Balance { get; }

		public CreditcardSnapshot(string number, decimal balance)
		{
			Number = number;
			Balance = balance;
		}
	}
}