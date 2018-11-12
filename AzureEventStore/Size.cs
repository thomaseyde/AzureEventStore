namespace AzureEventStore
{
    public class Size
    {
        public static Size Of(ushort value)
        {
            return new Size(value);
        }

        readonly ushort value;

        Size(ushort value)
        {
            this.value = value;
        }

        public static explicit operator ushort(Size v) => v.value;
    }
}