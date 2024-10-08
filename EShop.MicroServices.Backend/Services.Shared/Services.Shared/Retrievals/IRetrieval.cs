namespace Services.Shared.Retrievals;

public interface IRetrieval<T, TEntity> where TEntity : class where T : notnull
{
	Task<TEntity?> TryRetrieve(T searchParameter, CancellationToken ct);

	Task<TEntity[]> RetrieveBatch(T[] searchParameters, CancellationToken ct);
}