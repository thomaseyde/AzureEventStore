using System.Collections.Generic;

namespace Aggregates.Persistence
{
	public class AggregateHistory
	{
		private readonly uint version;
		private readonly ISnapshot snapshot;
		private readonly IEnumerable<object> history;

		public AggregateHistory(uint version, ISnapshot snapshot, IEnumerable<object> history)
		{
			this.version = version;
			this.snapshot = snapshot;
			this.history = history;
		}

		public AggregateHistory(IEnumerable<object> history)
		{
			this.history = history;
		}

		public void Apply(ILoadEvents loadable)
		{
			loadable.LoadFromEvents(history);
		}

		public void Apply(ILoadSnapshot root)
		{
			if (snapshot != null)
			{
				root.LoadFromSnapshot(version, snapshot);
			}

			root.LoadFromEvents(history);
		}
	}
}