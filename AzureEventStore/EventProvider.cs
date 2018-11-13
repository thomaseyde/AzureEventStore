using System.Linq;

namespace AzureEventStore
{
	public abstract class EventProvider
	{
		protected abstract void AppendCheckpoint(EventData data);
		protected abstract void AppendEvent(string key, EventData data);
		protected abstract EventData[] ReadForward(ushort size, string fromId, string toId);
		protected abstract EventData[] ReadBackward(ushort size, string fromId, string toId);

		public void Write(Stream stream, params EventData[] datas)
		{
			foreach (var data in datas)
			{
				AppendEvent(EventId(stream, data.Version), data);
				AppendCheckpoint(data);
			}
		}

		public EventSlice ReadForward(Stream stream, Size size, EventVersion fromVersion)
		{
			var fromId = EventId(stream, fromVersion);
			var toId = EventId(stream, EventVersion.Last);
			var slicedEvents = ReadForward((ushort) size, fromId, toId);
			return EventSlice.Next(slicedEvents);
		}

		static string EventId(Stream stream, uint version)
		{
			return $"{stream.Bucket}-{stream.Id}-{version:D10}";
		}

		public EventSlice ReadBackward(Stream stream, Size size, EventVersion fromVersion)
		{
			var fromId = EventId(stream, EventVersion.First);
			var toId = EventId(stream, fromVersion);
			var slicedEvents = ReadBackward((ushort) size, fromId, toId);
			return EventSlice.Previous(slicedEvents);
		}

		public CheckpointSlice Read(Size size, EventPosition fromPosition)
		{
			var slicedEvents = ReadForward((ushort) size, fromPosition, EventPosition.Last);

			if (slicedEvents.Any())
			{
				return CheckpointSlice.Next(slicedEvents);
			}

			return CheckpointSlice.Next(fromPosition);
		}

		protected abstract EventData[] ReadForward(ushort size, ulong fromPosition, ulong toPosition);
	}
}