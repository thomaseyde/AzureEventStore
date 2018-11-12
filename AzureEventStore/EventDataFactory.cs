using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureEventStore
{
    public class EventDataFactory
    {
	    public static EventDataFactory Create(IResolveTypes resolver)
	    {
		    var serializer = new EventSerializer();
		    return new EventDataFactory(resolver, serializer);
	    }

	    private EventDataFactory(IResolveTypes resolver, ISerializeEvents serializer)
        {
            this.resolver = resolver;
            this.serializer = serializer;
        }

	    public EventData Create(object e, uint version)
        {
            var type = resolver.GetTypename(e);
            var data = serializer.Serialize(e);
            var checkpoint = (ulong) DateTimeOffset.UtcNow.Ticks;
            return new EventData(type, data, checkpoint, version);
        }

	    public object Deserialize<T>(EventData data) where T : new()
	    {
		    return Deserialize(data, typeof(T));
	    }

	    public IEnumerable<object> Deserialize<T>(IEnumerable<EventData> events) where T : new()
	    {
		    return events.Select(data => Deserialize(data, typeof(T)));
	    }

	    private object Deserialize(EventData eventData, Type owner)
	    {
		    return serializer.Deserialize(resolver.GetType(eventData.Type, owner), eventData.Data);
	    }

	    readonly IResolveTypes resolver;
	    readonly ISerializeEvents serializer;
    }
}