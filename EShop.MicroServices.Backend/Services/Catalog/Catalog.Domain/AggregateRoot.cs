namespace Catalog.Domain
{
	public abstract class AggregateRoot<T> where T : struct
	{
		public T Id { get; protected set; }
	}
}