using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureTableStorageStuff.Library
{
    public class MemoryProvider
    {
        private readonly Dictionary<ulong, EventData> _checkpoints;
        
        private readonly Dictionary<string, EventData> _events = new Dictionary<string, EventData>();

        public MemoryProvider()
        {
            _checkpoints = new Dictionary<ulong, EventData>();
        }

        public IEnumerable<EventData> ReadForward(Stream stream, uint fromVersion)
        {
            var fromId = EventId(stream, fromVersion);
            var toId = EventId(stream, uint.MaxValue);

            var versions = (IEnumerable<string>) _events.Keys
                .Where(version => string.Compare(fromId, version, StringComparison.Ordinal) <= 0 &&
                                  string.Compare(version, toId, StringComparison.Ordinal) <= 0)
                .OrderBy(version => version);

            foreach (var version in versions)
            {
                yield return _events[version];
            }
        }

        public void Write(string eventId, EventData data)
        {
            _events.Add(eventId, data);
        }

        public void Write(ulong checkpoint, EventData e)
        {
            _checkpoints.Add(checkpoint, e);
        }

        public static string EventId(Stream stream, uint version)
        {
            return Key(stream) + "-" + version.ToString("D10");
        }

        private static string Key(Stream id)
        {
            return id.Bucket + "-" + id.Id;
        }
    }
}