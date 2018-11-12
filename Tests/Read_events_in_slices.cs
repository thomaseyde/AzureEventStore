using System.Collections.Generic;
using System.Linq;
using AzureEventStore;
using AzureEventStore.Testing;
using ExampleDomain;
using ExampleDomain.Persistence;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class Read_events_in_slices
    {
	    readonly Stream stream;
        readonly Size size;

        public Read_events_in_slices()
        {
	        var store = new EventStore(new MemoryProvider());
            stream = store.OpenStream("stream", "id");
            var factory = EventDataFactory.Create(new TypeResolver());

            var events = Enumerable.Range(1, 100)
                .Select(version => new StoreTested(version))
                .Select(tested => factory.Create(tested, EventVersion.From(tested.Version)));

	        stream.Write(events);
	        size = Size.Of(10);
        }

        [Fact]
        public void Read_slice_forward_from_start()
        {
            var slice = stream.ReadForward(size, EventVersion.First);

	        slice.Events.Length.Should().Be(10);
	        slice.NextVersion.Should().Be(EventVersion.From(11));
        }

        [Fact]
        public void Read_slice_forward_from_version()
        {
            var slice = stream.ReadForward(size, EventVersion.From(11));

	        slice.Events.Length.Should().Be(10);
	        slice.NextVersion.Should().Be(EventVersion.From(21));
        }

        [Fact]
        public void Read_slice_forward_from_end()
        {
            var slice = stream.ReadForward(size, EventVersion.Last);

	        slice.Events.Should().BeEmpty();
	        slice.NextVersion.Should().Be(EventVersion.None);
        }

        [Fact]
        public void Read_all_slices_forward()
        {
            var events = new List<EventData>();
            var version = EventVersion.First;

	        EventSlice slice;
	        do
            {
                slice = stream.ReadForward(size, version);

	            events.AddRange(slice.Events);

	            version = slice.NextVersion;
            } while (slice.HasMore());

	        events.Count.Should().Be(100);
	        events.First().Version.Should().Be(1);
	        events.Last().Version.Should().Be(100);
        }

        [Fact]
        public void Read_all_slices_backward()
        {
            var events = new List<EventData>();
            var fromVersion = EventVersion.Last;

	        EventSlice slice;
	        do
            {
                slice = stream.ReadBackward(size, fromVersion);

	            foreach (var data in slice.Events)
	            {
		            events.Insert(0, data);
	            }

	            fromVersion = slice.NextVersion;
            } while (slice.HasMore());

	        events.Count.Should().Be(100);
	        events.First().Version.Should().Be(1);
	        events.Last().Version.Should().Be(100);
        }

	    [Fact]
        public void Forward_slice_should_be_in_rising_order()
        {
            var slice = stream.ReadForward(size, EventVersion.First);
	        slice.Events.First().Version.Should().Be(1);
	        slice.Events.Last().Version.Should().Be(10);
        }

        [Fact]
        public void Read_slice_backward_from_start()
        {
            var slice = stream.ReadBackward(size, EventVersion.First);

	        slice.Events.Should().BeEmpty();
	        slice.NextVersion.Should().Be(EventVersion.None);
        }

        [Fact]
        public void Read_slice_backwards_from_end()
        {
            var slice = stream.ReadBackward(size, EventVersion.Last);
	        slice.Events.Length.Should().Be(10);
	        slice.NextVersion.Should().Be(EventVersion.From(90));
        }

        [Fact]
        public void Slice_should_include_version_info()
        {
            var forward = stream.ReadForward(size, EventVersion.From(10));
	        forward.NextVersion.Should().Be(EventVersion.From(20));
	        forward.Events.First().Version.Should().Be(10);
	        forward.Events.Last().Version.Should().Be(19);

	        var backward = stream.ReadBackward(size, EventVersion.From(10));
	        backward.NextVersion.Should().Be(EventVersion.From(0));
	        backward.Events.First().Version.Should().Be(10);
	        backward.Events.Last().Version.Should().Be(1);
        }

        [Fact]
        public void Backward_slice_should_be_in_rising_order()
        {
            var slice = stream.ReadBackward(size, EventVersion.Last);
	        slice.Events.First().Version.Should().Be(100);
	        slice.Events.Last().Version.Should().Be(91);
        }
    }
}