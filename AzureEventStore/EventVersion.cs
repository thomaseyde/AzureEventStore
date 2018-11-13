using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureEventStore 
{
    public class EventVersion : IEquatable<EventVersion>, IComparable<EventVersion>
    {
        public static readonly EventVersion None = new EventVersion(uint.MinValue);

	    public static readonly EventVersion First = new EventVersion(1);

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

	    public static implicit operator uint(EventVersion v) => v.value;

	    public static bool operator ==(EventVersion left, EventVersion right)
        {
            return Equals(left, right);
        }

	    public static bool operator !=(EventVersion left, EventVersion right)
        {
            return !Equals(left, right);
        }

	    public int CompareTo(EventVersion other)
	    {
		    if (ReferenceEquals(this, other)) return 0;
		    if (ReferenceEquals(null, other)) return 1;

		    return (value).CompareTo(other.value);
	    }
    }
}