namespace Services.Shared.Storage;

public interface IStorage<TEntity> where TEntity : class
{
	Task Store(TEntity entity);

	Task StoreUpdate(TEntity entity);
}