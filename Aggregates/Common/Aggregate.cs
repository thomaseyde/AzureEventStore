using System.Collections.Generic;
using Aggregates.Persistence;

namespace Aggregates.Common
{
	public abstract class Aggregate
	{
		private readonly List<object> changes = new List<object>();
		public abstract string Id { get; }
		public IEnumerable<object> GetChanges() => changes;
		public void CommitChanges() => changes.Clear();

		protected AggregateVersion Version { get; set; } = AggregateVersion.None;

		public void LoadFromEvents(IEnumerable<object> events)
		{
			foreach (var e in events)
			{
				ApplyEvent(e);
				Version = Version.Add(1);
			}
		}

		public AggregateVersion GetVersion(object change)
		{
			if (change is ISnapshot) return Version.Add(GetSnapshotSequence());
			return Version.Add(GetEventSequence(change));
		}

		protected void Raise(object e)
		{
			changes.Add(e);
			ApplyEvent(e);
		}

		protected bool NeedSnapshot(int limit)
		{
			var snapshotVersion = Version.Modulo(limit).Add(changes.Count);

			return snapshotVersion.GreaterOrEqual(limit);
		}

		protected abstract void ApplyEvent(object e);

		private int GetEventSequence(object change)
		{
			return changes.IndexOf(change) + 1;
		}

		private int GetSnapshotSequence()
		{
			return changes.Count + 1;
		}
	}
}