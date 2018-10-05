using System;
using Newtonsoft.Json;

namespace AzureTableStorageStuff.Library
{
    public class EventData
    {
        public static EventData Create(object e)
        {
            var type = e.GetType().Name;
            var data = JsonConvert.SerializeObject(e);
            var checkpoint = (ulong) DateTimeOffset.UtcNow.Ticks;
            return new EventData(type, data, checkpoint);
        }

        public string Type { get; }
        public string Data { get; }
        public ulong Checkpoint { get; }

        public EventData(string type, string data, ulong checkpoint)
        {
            Type = type;
            Data = data;
            Checkpoint = checkpoint;
        }
    }
}