using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureEventStore 
{
    public class EventVersion : IEquatable<EventVersion>, IComparable<EventVersion>
    {
        public static readonly EventVersion None = new EventVersion(uint.MinValue);

	    public static readonly EventVersion First = new EventVersion(uint.MinValue);

	    public static readonly EventVersion Last = new EventVersion(uint.MaxValue);

	    public static EventVersion Next(ICollection<EventData> events)
        {
            if (!events.Any()) return None;
            var next = events.Max(data => data.Version) + 1;
            return new EventVersion(next);
        }

	    public static EventVersion Previous(ICollection<EventData> events)
        {
            if (!events.Any()) return None;
            var previous = events.Min(data => data.Version) - 1;
            return new EventVersion(previous);
        }

	    public static EventVersion From(int version)
	    {
		    return new EventVersion((uint) version);
	    }

	    public static EventVersion From(uint version)
	    {
		    return new EventVersion(version);
	    }

	    public override string ToString()
        {
            return value.ToString();
        }

	    readonly uint value;

	    private EventVersion(uint value)
        {
            this.value = value;
        }

	    public bool Equals(EventVersion other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return value == other.value;
        }

	    public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((EventVersion) obj);
        }

	    public override int GetHashCode()
        {
            return (int) value;
        }

	    public EventVersion Modulo(int divisor)
	    {
		    return From((uint) (value % divisor));
	    }

	    public EventVersion Add(int addend)
	    {
		    return From((uint) (value + addend));
	    }

	    public bool GreaterOrEqual(int comparand)
	    {
		    return value >= comparand;
	    }

	    public static implicit operator uint(EventVersion v) => v.value;

	    public static bool operator ==(EventVersion left, EventVersion right)
        {
            return Equals(left, right);
        }

	    public static bool operator !=(EventVersion left, EventVersion right)
        {
            return !Equals(left, right);
        }

	    public string ToString(byte digits)
        {
            // ReSharper disable once ImpureMethodCallOnReadonlyValueField
            return value.ToString($"D{digits}");
        }

	    public int CompareTo(EventVersion other)
	    {
		    if (ReferenceEquals(this, other)) return 0;
		    if (ReferenceEquals(null, other)) return 1;

		    return (value).CompareTo(other.value);
	    }
    }
}