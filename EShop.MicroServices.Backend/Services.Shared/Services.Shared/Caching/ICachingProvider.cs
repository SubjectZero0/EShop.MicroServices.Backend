namespace Services.Shared.Caching
{
	public interface ICachingProvider<TKey, TEntity>
	{
		Task SetToCache(TKey key, TEntity entity);

		Task<TEntity?> TryGetFromCache(TKey key);
	}
}