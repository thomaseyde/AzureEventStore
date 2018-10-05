using System.Collections.Generic;
using System.Linq;

namespace AzureTableStorageStuff.Library
{
    public class EventStore
    {
        private readonly MemoryProvider _provider;

        public EventStore()
        {
            _provider = new MemoryProvider();
        }

        public void Write(Stream stream, uint expectedVersion, IEnumerable<EventData> events)
        {
            Write(stream, expectedVersion, events.ToArray());
        }

        public void Write(Stream stream, uint expectedVersion, params EventData[] events)
        {
            foreach (var offset in events.Select((e, i) => new {i, e}))
            {
                var version = (uint) (expectedVersion + offset.i);
                _provider.Write(MemoryProvider.EventId(stream, version), offset.e);
                _provider.Write(offset.e.Checkpoint, offset.e);
            }
        }

        public IEnumerable<EventData> ReadForward(Stream stream, uint fromVersion)
        {
            return _provider.ReadForward(stream, fromVersion);
        }

        public IEnumerable<EventData> ReadBackwards(Stream stream, uint fromVersion)
        {
            return _provider.ReadBackwards(stream, fromVersion);
        }
    }
}