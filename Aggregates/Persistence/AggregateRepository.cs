using System.Collections.Generic;

namespace Aggregates.Persistence
{
	public abstract class AggregateRepository
	{
		public void Save<T>(T aggregate)
		{
			switch (aggregate)
			{
				case IProduceSnapshot producer:
					Save(producer);
					break;
				case IProduceEvents producer:
					Save(producer);
					break;
			}
		}

		private void Save(IProduceSnapshot aggregate)
		{
			var history = aggregate.GetChanges();
			var snapshot = aggregate.CreateSnapshot();

			Save(aggregate, history, snapshot);

			aggregate.CommitChanges();
		}

		protected abstract void Save(IProduceSnapshot aggregate, IEnumerable<object> history, ISnapshot snapshot);

		private void Save(IProduceEvents aggregate)
		{
			var history = aggregate.GetChanges();

			Save(aggregate, history);

			aggregate.CommitChanges();
		}

		protected abstract void Save(IProduceEvents aggregate, IEnumerable<object> history);

		public T Get<T>(string id) where T : new()
		{
			var root = new T();

			switch (root)
			{
				case ILoadSnapshot loadable:
					GetSnapshotHistory<T>(id).Apply(loadable);
					break;
				case ILoadEvents loadable:
					GetEventHistory<T>(id).Apply(loadable);
					break;
			}

			return root;
		}

		protected abstract AggregateHistory GetEventHistory<T>(string id) where T : new();
		protected abstract AggregateHistory GetSnapshotHistory<T>(string id) where T : new();
	}
}