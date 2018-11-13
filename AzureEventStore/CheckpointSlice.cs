namespace AzureEventStore
{
	public class CheckpointSlice
	{
		public static CheckpointSlice Next(EventData[] events)
		{
			return new CheckpointSlice(events, EventPosition.Next(events));
		}

		//public static CheckpointSlice Previous(EventData[] events)
		//{
		//	return new CheckpointSlice(events, EventVersion.Previous(events));
		//}

		public EventData[] Events { get; }
		public EventPosition NextPosition { get; }

		CheckpointSlice(EventData[] events, EventPosition next)
		{
			Events = events;
			NextPosition = next;
		}

		public bool HasMore()
		{
			return NextPosition != EventVersion.None;
		}
	}
}