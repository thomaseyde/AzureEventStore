using System.Collections.Generic;
using System.Linq;
using Aggregates.Persistence;
using AzureEventStore;

namespace ExampleDomain
{
	public class Repository : AggregateRepository
	{
		private readonly string bucket;
		private readonly EventDataFactory factory;
		private readonly EventStore store;

		public Repository(string bucket, IResolveTypes resolver, EventProvider provider)
		{
			this.bucket = bucket;
			store = new EventStore(provider);
			factory = EventDataFactory.Create(resolver);
		}

		protected override AggregateHistory GetEventHistory<T>(string id)
		{
			var stream = store.OpenStream(bucket, id);
			var version = EventVersion.First;
			var history = new List<object>();

			EventSlice slice;
			do
			{
				slice = stream.ReadForward(Size.Of(100), version);
				history.AddRange(factory.Deserialize<T>(slice.Events));

				version = slice.NextVersion;
			} while (slice.HasMore());

			return new AggregateHistory(history);
		}

		protected override AggregateHistory GetSnapshotHistory<T>(string id)
		{
			var stream = store.OpenStream(bucket, id);
			var history = new Stack<object>();
			var version = EventVersion.Last;

			EventSlice slice;
			do
			{
				slice = stream.ReadBackward(Size.Of(100), version);

				foreach (var data in slice.Events)
				{
					var e = factory.Deserialize<T>(data);
					if (e is ISnapshot snapshot) return new AggregateHistory(data.Version, snapshot, history);

					history.Push(e);
				}

				version = slice.NextVersion;
			} while (slice.HasMore());

			return new AggregateHistory(history);
		}

		protected override void Save(IProduceEvents aggregate, IEnumerable<object> history)
		{
			var changes = history
			              .Select((e, index) => factory.Create(e, aggregate.GetVersion(e)))
			              .ToList();

			store.OpenStream(bucket, aggregate.Id).Write(changes);
		}

		protected override void Save(IProduceSnapshot aggregate, IEnumerable<object> history, ISnapshot snapshot)
		{
			var changes = history
			              .Select((e, index) => factory.Create(e, aggregate.GetVersion(e)))
			              .ToList();

			if (snapshot != Snapshot.None) changes.Add(factory.Create(snapshot, aggregate.GetVersion(snapshot)));

			store.OpenStream(bucket, aggregate.Id).Write(changes);
		}
	}
}