namespace Services.Shared.Storage;

public interface IStorage<TEntity> where TEntity : class
{
	Task StoreNew(TEntity entity, CancellationToken ct);

	Task StoreUpdate(TEntity entity, CancellationToken ct);
}