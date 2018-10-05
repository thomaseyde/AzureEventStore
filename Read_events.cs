using System.Linq;
using AzureTableStorageStuff.Library;
using Xunit;

namespace AzureTableStorageStuff
{
    public class Read_events
    {
        private readonly EventStore _store;
        private readonly Stream _stream;

        public Read_events()
        {
            _store = new EventStore();
            _stream = new Stream("stream", "id");

            var events = Enumerable.Range(1, 10)
                .Select(i => new StoreTested(i))
                .Select(EventData.Create);

            _store.Write(_stream, 1, events);
        }

        [Fact]
        public void Read_backwards_from_end()
        {
            var events = _store.ReadBackwards(_stream, uint.MaxValue);
            Assert.Equal(10, events.Count());
        }

        [Fact]
        public void Read_backwards_from_version()
        {
            var events = _store.ReadBackwards(_stream, 9);
            Assert.Equal(9, events.Count());
        }

        [Fact]
        public void Read_forward_from_start()
        {
            var events = _store.ReadForward(_stream, 1);

            Assert.Equal(10, events.Count());
        }

        [Fact]
        public void Read_forward_from_version()
        {
            var events = _store.ReadForward(_stream, 2);

            Assert.Equal(9, events.Count());
        }
    }
}