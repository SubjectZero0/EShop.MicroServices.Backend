namespace Services.Shared.Retrievals;

public interface IRetrieval<in T, out TEntity> where TEntity: class
{
    TEntity? TryRetrieve(T searchParameter);
    TEntity[] RetrieveBatch(T[] searchParameters);
}