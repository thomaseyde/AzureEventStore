namespace AzureEventStore
{
    public class EventData
    {
        public string Type { get; }
        public string Data { get; }
        public ulong Checkpoint { get; }
        public uint Version { get; }

        public EventData(string type, string data, ulong checkpoint, uint version)
        {
            Type = type;
            Data = data;
            Checkpoint = checkpoint;
            Version = version;
        }
    }
}