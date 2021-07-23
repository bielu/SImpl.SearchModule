namespace SImpl.SearchModule.Abstraction.Queries
{
    public interface ICreatableSearchQuery<TKey,TValue>
    {
        void Add(TKey key, TValue value);
    }
}