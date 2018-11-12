using Aggregates.Common;
using Aggregates.Persistence;

namespace Aggregates
{
	public abstract class EventSourceAggregate : Aggregate, ILoadEvents, IProduceEvents
	{
	}
}