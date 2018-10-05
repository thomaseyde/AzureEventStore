using System;
using System.Collections.Generic;
using System.Linq;
using AzureTableStorageStuff.Library;
using Newtonsoft.Json;
using Xunit;

namespace AzureTableStorageStuff
{
    public class Write_events
    {
        private readonly EventStore _store;
        private readonly Stream _stream;

        public Write_events()
        {
            _store = new EventStore();
            _stream = new Stream("stream", "id");
        }

        [Fact]
        public void Write_single_events()
        {
            _store.Write(_stream, 1, SingleEvent(1));
            _store.Write(_stream, 2, SingleEvent(2), SingleEvent(3));

            Assert.Equal(3, Count());
        }

        [Fact]
        public void Write_multiple_events()
        {
            _store.Write(_stream, 1, MultipleEvents());

            Assert.Equal(2, Count());
        }

        private int Count()
        {
            return _store.ReadForward(_stream, 1).Count();
        }

        private static IEnumerable<EventData> MultipleEvents()
        {
            yield return SingleEvent(1);
            yield return SingleEvent(2);
        }

        private static EventData SingleEvent(int version)
        {
            return new EventData("StoreTested", JsonConvert.SerializeObject(new StoreTested(version)), (ulong) DateTimeOffset.UtcNow.Ticks);
        }
    }
}