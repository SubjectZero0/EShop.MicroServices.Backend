using System.Text.Json.Serialization;

namespace Basket.Domain.Aggregates;

public abstract class AggregateRoot<T> where T: struct
{
    [JsonInclude]
    public T Id { get; private set; }

    protected AggregateRoot(T id)
    {
        Id = id;
    }
}