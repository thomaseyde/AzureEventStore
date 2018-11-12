using System;
using Newtonsoft.Json;

namespace AzureEventStore
{
    public class EventSerializer : ISerializeEvents
    {
        public string Serialize(object e)
        {
            return JsonConvert.SerializeObject(e);
        }

        public object Deserialize(Type type, string data)
        {
            return JsonConvert.DeserializeObject(data, type);
        }
    }
}