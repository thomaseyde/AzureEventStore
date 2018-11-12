using System.Collections.Generic;
using Aggregates.Common;

namespace Aggregates.Persistence
{
	public interface IProduceEvents
	{
		IEnumerable<object> GetChanges();
		void CommitChanges();
		string Id { get; }
		AggregateVersion GetVersion(object e);
	}
}