namespace Aggregates.Persistence
{
	public class Snapshot : ISnapshot
	{
		public static readonly ISnapshot None = new Snapshot();
	}
}