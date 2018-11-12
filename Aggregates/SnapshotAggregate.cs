using Aggregates.Common;
using Aggregates.Persistence;

namespace Aggregates
{
	public abstract class SnapshotAggregate<TSnapshot> : Aggregate, ILoadSnapshot, IProduceSnapshot 
		where TSnapshot : ISnapshot
	{
		public void LoadFromSnapshot(uint version, object snapshot)
		{
			ApplySnapshot((TSnapshot) snapshot);
			Version = new AggregateVersion(version);
		}

		public ISnapshot CreateSnapshot()
		{
			return NeedSnapshot(versionsPerSnapshot) ? TakeSnapshot() : Snapshot.None;
		}

		protected abstract void ApplySnapshot(TSnapshot snapshot);

		protected abstract TSnapshot TakeSnapshot();

		protected SnapshotAggregate(ushort versionsPerSnapshot)
		{
			this.versionsPerSnapshot = versionsPerSnapshot;
		}

		readonly ushort versionsPerSnapshot;
	}
}