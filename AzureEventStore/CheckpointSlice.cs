using System.Linq;

namespace AzureEventStore
{
	public class CheckpointSlice
	{
		public static CheckpointSlice Next(EventData[] events)
		{
			return new CheckpointSlice(events, EventPosition.Next(events));
		}

		public EventData[] Events { get; }
		public EventPosition NextPosition { get; }

		CheckpointSlice(EventData[] events, EventPosition next)
		{
			Events = events;
			NextPosition = next;
		}

		public bool HasMore()
		{
			return Events.Any();
		}

		public static CheckpointSlice Next(EventPosition fromPosition)
		{
			return new CheckpointSlice(new EventData[0], fromPosition);
		}
	}
}