namespace AzureEventStore
{
	public class CheckpointStream
	{
		private readonly EventProvider provider;

		public CheckpointStream(EventProvider provider)
		{
			this.provider = provider;
		}

		//public void Write(IEnumerable<EventData> changes)
		//{
		//	provider.Write(this, changes.ToArray());
		//}

		public CheckpointSlice Read(Size size, EventPosition fromPosition)
		{
			return provider.Read(size, fromPosition);
		}
	}
}