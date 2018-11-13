using System.Collections.Generic;
using System.Linq;

namespace AzureEventStore
{
    public class Stream
    {
	    private readonly EventProvider provider;
	    public string Bucket { get; }
        public string Id { get; }

	    public Stream(string bucket, string id, EventProvider provider)
	    {
		    this.provider = provider;
		    Bucket = bucket;
		    Id = id;
	    }

	    public void Write(IEnumerable<EventData> changes)
	    {
		    provider.Write(this, changes.ToArray());
	    }

	    public EventSlice ReadForward(Size size, EventVersion fromVersion)
	    {
		    return provider.ReadForward(this, size, fromVersion);
	    }

	    public EventSlice ReadBackward(Size size, EventVersion fromVersion)
	    {
		    return provider.ReadBackward(this, size, fromVersion);
	    }
    }
}