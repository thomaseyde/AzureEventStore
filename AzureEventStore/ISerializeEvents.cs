using System;

namespace AzureEventStore
{
    public interface ISerializeEvents
    {
        string Serialize(object e);
        object Deserialize(Type type, string data);
    }
}