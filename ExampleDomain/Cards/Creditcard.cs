using System;
using Aggregates;

namespace ExampleDomain.Cards
{
	public class Creditcard : SnapshotAggregate<CreditcardSnapshot>
	{
		public static Creditcard Issue(string creditcardNumber)
		{
			var card = new Creditcard();
			card.Raise(new CreditcardIssued(creditcardNumber));
			return card;
		}

		string number;

		public Creditcard() : base(5)
		{
		}

		public decimal Balance { get; private set; }
		public override string Id => number;

		protected override void ApplySnapshot(CreditcardSnapshot snapshot)
		{
			number = snapshot.Number;
			Balance = snapshot.Balance;
		}

		protected override CreditcardSnapshot TakeSnapshot()
		{
			return new CreditcardSnapshot(number, Balance);
		}

		public void PostTransaction(decimal amount)
		{
			Raise(new CreditcardTransactionPosted(number, Math.Abs(amount)));
		}

		public void PayIssuer(decimal amount)
		{
			Raise(new CreditcardIssuerPaid(number, Math.Abs(amount)));
		}

		protected override void ApplyEvent(object e)
		{
			switch (e)
			{
				case CreditcardIssued acquired:
					Apply(acquired);
					break;
				case CreditcardTransactionPosted withdrawn:
					Apply(withdrawn);
					break;
				case CreditcardIssuerPaid deposited:
					Apply(deposited);
					break;
			}
		}

		void Apply(CreditcardIssued e) => number = e.CreditcardNumber;
		void Apply(CreditcardTransactionPosted e) => Balance -= e.Amount;
		void Apply(CreditcardIssuerPaid e) => Balance += e.Amount;
	}
}