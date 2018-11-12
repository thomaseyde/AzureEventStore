namespace AzureEventStore
{
    public class EventSlice
    {
        public static EventSlice Next(EventData[] events)
        {
            return new EventSlice(events, EventVersion.Next(events));
        }

        public static EventSlice Previous(EventData[] events)
        {
            return new EventSlice(events, EventVersion.Previous(events));
        }

        public EventData[] Events { get; }
        public EventVersion NextVersion { get; }

        EventSlice(EventData[] events, EventVersion next)
        {
            Events = events;
            NextVersion = next;
        }

	    public bool HasMore()
	    {
		    return NextVersion != EventVersion.None;
	    }
    }
}