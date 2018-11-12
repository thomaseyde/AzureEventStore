namespace Aggregates.Common 
{
    public class AggregateVersion 
    {
        public static readonly AggregateVersion None = new AggregateVersion(uint.MinValue);

	    public override string ToString()
        {
            return value.ToString();
        }

	    readonly uint value;

	    public AggregateVersion(uint value)
        {
            this.value = value;
        }

	    public AggregateVersion Modulo(int divisor)
	    {
		    return new AggregateVersion((uint) (value % divisor));
	    }

	    public AggregateVersion Add(int addend)
	    {
		    return new AggregateVersion((uint) (value + addend));
	    }

	    public bool GreaterOrEqual(int comparand)
	    {
		    return value >= comparand;
	    }

	    public static implicit operator uint(AggregateVersion v) => v.value;
    }
}