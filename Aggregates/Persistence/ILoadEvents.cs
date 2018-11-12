using System.Collections.Generic;

namespace Aggregates.Persistence
{
	public interface ILoadEvents
	{
		void LoadFromEvents(IEnumerable<object> events);
	}
}