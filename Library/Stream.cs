namespace AzureTableStorageStuff.Library
{
    public class Stream
    {
        public string Bucket { get; }
        public string Id { get; }

        public Stream(string bucket, string id)
        {
            Bucket = bucket;
            Id = id;
        }
    }
}