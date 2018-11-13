using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureEventStore
{
	public class EventPosition : IEquatable<EventPosition>, IComparable<EventPosition>
	{
		public static readonly EventPosition None = new EventPosition(ulong.MinValue);

		public static readonly EventPosition First = new EventPosition(1);

		public static readonly EventPosition Last = new EventPosition(ulong.MaxValue);

		public static EventPosition Next(ICollection<EventData> events)
		{
			if (!events.Any()) return None;
			var next = events.Max(data => data.Checkpoint) + 1;
			return new EventPosition(next);
		}

		public static EventPosition Previous(ICollection<EventData> events)
		{
			if (!events.Any()) return None;
			var previous = events.Min(data => data.Version) - 1;
			return new EventPosition(previous);
		}

		public static EventPosition From(long version)
		{
			return new EventPosition((ulong) version);
		}

		public static EventPosition From(ulong version)
		{
			return new EventPosition(version);
		}

		public override string ToString()
		{
			return value.ToString();
		}

		readonly ulong value;

		private EventPosition(ulong value)
		{
			this.value = value;
		}

		public bool Equals(EventPosition other)
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
			return Equals((EventPosition) obj);
		}

		public override int GetHashCode()
		{
			return (int) value;
		}

		public EventPosition Modulo(ulong divisor)
		{
			return From((ulong) (value % divisor));
		}

		public EventPosition Add(ulong addend)
		{
			return From((ulong) (value + addend));
		}

		public bool GreaterOrEqual(ulong comparand)
		{
			return value >= comparand;
		}

		public static implicit operator ulong(EventPosition v) => v.value;

		public static bool operator ==(EventPosition left, EventPosition right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(EventPosition left, EventPosition right)
		{
			return !Equals(left, right);
		}

		public string ToString(byte digits)
		{
			// ReSharper disable once ImpureMethodCallOnReadonlyValueField
			return value.ToString($"D{digits}");
		}

		public int CompareTo(EventPosition other)
		{
			if (ReferenceEquals(this, other)) return 0;
			if (ReferenceEquals(null, other)) return 1;

			return (value).CompareTo(other.value);
		}
	}
}