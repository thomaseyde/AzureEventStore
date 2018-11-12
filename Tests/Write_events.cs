using System;
using System.Collections.Generic;
using AzureEventStore;
using AzureEventStore.Testing;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Tests
{
    public class Write_events
    {
        readonly EventStore store;
        readonly Stream stream;

        public Write_events()
        {
            store = new EventStore(new MemoryProvider());
            stream = store.OpenStream("stream", "id");
        }

        [Fact]
        public void Write_single_events()
        {
	        stream.Write(new[] {SingleEvent(1)});
	        stream.Write(new[] {SingleEvent(2), SingleEvent(3)});

	        Count().Should().Be(3);
        }

        [Fact]
        public void Write_multiple_events()
        {
	        stream.Write(MultipleEvents());

	        Count().Should().Be(2);
        }

        int Count()
        {
            return stream.ReadForward(Size.Of(10), EventVersion.First).Events.Length;
        }

        static IEnumerable<EventData> MultipleEvents()
        {
            yield return SingleEvent(1);
            yield return SingleEvent(2);
        }

        static EventData SingleEvent(int version)
        {
            var e = new StoreTested(version);
            return new EventData(e.GetType().Name, JsonConvert.SerializeObject(e), (ulong) DateTimeOffset.UtcNow.Ticks, EventVersion.From(version));
        }
    }
}