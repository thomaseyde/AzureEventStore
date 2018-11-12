using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureEventStore.Testing
{
	public class MemoryProvider : EventProvider
	{
		private readonly Dictionary<ulong, EventData> checkpoints;
		private readonly Dictionary<string, EventData> events = new Dictionary<string, EventData>();

		public MemoryProvider()
		{
			checkpoints = new Dictionary<ulong, EventData>();
		}

		protected override void AppendEvent(string key, EventData data)
		{
			events.Add(key, data);
		}

		protected override void AppendCheckpoint(EventData data)
		{
			checkpoints.Add(data.Checkpoint, data);
		}

		protected override EventData[] ReadForward(ushort size, string fromId, string toId)
		{
			return events.Keys
			             .Where(version => Between(fromId, version, toId))
			             .OrderBy(version => version)
			             .Take(size)
			             .Select(version => events[version])
			             .ToArray();
		}

		protected override EventData[] ReadBackward(ushort size, string fromId, string toId)
		{
			return events.Keys
			             .Where(version => Between(fromId, version, toId))
			             .OrderByDescending(version => version)
			             .Take(size)
			             .Select(version => events[version])
			             .ToArray();
		}

		private static bool Between(string start, string value, string end)
		{
			return string.Compare(start, value, StringComparison.Ordinal) <= 0 &&
			       string.Compare(value, end, StringComparison.Ordinal) <= 0;
		}
	}
}