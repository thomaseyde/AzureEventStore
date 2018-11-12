namespace AzureEventStore
{
    public class EventStore
    {
        readonly EventProvider provider;

        public EventStore(EventProvider provider)
        {
            this.provider = provider;
        }

	    public Stream OpenStream(string bucket, string id)
	    {
		    return new Stream(bucket, id, provider);
	    }
    }
}