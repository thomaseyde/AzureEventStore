using System.Collections.Generic;
using Aggregates.Common;

namespace Aggregates.Persistence
{
	public interface IProduceSnapshot
	{
		ISnapshot CreateSnapshot();
		void CommitChanges();
		string Id { get; }

		IEnumerable<object> GetChanges();
		AggregateVersion GetVersion(object e);
	}
}