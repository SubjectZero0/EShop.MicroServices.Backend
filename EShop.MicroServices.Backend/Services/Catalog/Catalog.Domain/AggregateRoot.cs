using System.Text.Json.Serialization;

namespace Catalog.Domain
{
	public abstract class AggregateRoot<T> where T : struct
	{
		[JsonInclude]
		public T Id { get; private set; }
		
		protected AggregateRoot(T id)
		{
			Id = id;
		}
	}
	
}