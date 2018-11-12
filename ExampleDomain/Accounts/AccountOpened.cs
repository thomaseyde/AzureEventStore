namespace ExampleDomain.Accounts
{
	class AccountOpened
	{
		public string Number { get; }

		public AccountOpened(string number)
		{
			Number = number;
		}
	}
}