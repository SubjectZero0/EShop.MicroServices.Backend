namespace Services.Shared.Retrievals;

public interface IRetrieval<T, TEntity> where TEntity: notnull
{
    Task<TEntity> TryRetrieve(T searchParameter);
    Task<TEntity> RetrieveBatch(T[] searchParameters);
}