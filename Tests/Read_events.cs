using System.Linq;
using AzureEventStore;
using AzureEventStore.Testing;
using ExampleDomain.Persistence;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class Read_events
    {
	    readonly Stream stream;
        readonly Size size;
	    private readonly EventStore store;

	    public Read_events()
        {
	        store = new EventStore(new MemoryProvider());
            stream = store.OpenStream("stream", "id");
            var factory = EventDataFactory.Create(new TypeResolver());

            var events = Enumerable.Range(1, 10)
                .Select(version => new {Event = new StoreTested(version), Version = EventVersion.From(version)})
                .Select(arg => factory.Create(arg.Event, arg.Version));

	        stream.Write(events);

	        size = Size.Of(10);
        }

        [Fact]
        public void Read_backwards_from_end()
        {
            var slice = stream.ReadBackward(size, EventVersion.Last);
	        slice.Events.Should().HaveCount(10);
        }

        [Fact]
        public void Read_backwards_from_version()
        {
            var slice = stream.ReadBackward(size, EventVersion.From(9));
	        slice.Events.Should().HaveCount(9);
        }

        [Fact]
        public void Read_forward_from_start()
        {
            var events = stream.ReadForward(size, EventVersion.First);

	        events.Events.Should().HaveCount(10);
        }

        [Fact]
        public void Read_forward_from_version()
        {
            var slice = stream.ReadForward(size, EventVersion.From(2));

	        slice.Events.Should().HaveCount(9);
        }

	    [Fact]
	    public void Read_checkpoints_from_start()
	    {
		    var checkpoints = store.OpenCheckpoints();
		    var slice = checkpoints.Read(size, EventPosition.First);
		    slice.Events.Should().HaveCount(10);
	    }

	    [Fact]
	    public void Read_checkpoints_from_position()
	    {
		    var checkpoints = store.OpenCheckpoints();
		    var skip = checkpoints.Read(Size.Of(1), EventPosition.First);
		    var slice = checkpoints.Read(size, EventPosition.From(skip.NextPosition));
		    slice.Events.Should().HaveCount(9);
	    }

	    [Fact]
	    public void Read_checkpoints_past_end()
	    {
		    var checkpoints = store.OpenCheckpoints();
		    
		    var all = checkpoints.Read(size, EventPosition.First);
		    all.HasMore().Should().BeTrue();

		    var slice = checkpoints.Read(size, EventPosition.From(all.NextPosition));
		    
		    slice.NextPosition.Should().Be(slice.NextPosition);
		    slice.NextPosition.Should().NotBe(EventPosition.None);
		    slice.NextPosition.Should().NotBe(EventPosition.Last);
		    slice.HasMore().Should().BeFalse();
	    }
    }
}