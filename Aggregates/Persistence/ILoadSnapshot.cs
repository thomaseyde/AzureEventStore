namespace Aggregates.Persistence
{
	public interface ILoadSnapshot : ILoadEvents
	{
		void LoadFromSnapshot(uint version, object snapshot);
	}
}