using MediatR;

namespace Services.Shared.CQRS
{
	public interface IQuery<out TResponse>
		: IRequest<TResponse>
		where TResponse : class?
	{
	}

	public interface IQueryHandler<in TQuery, TResponse>
		: IRequestHandler<TQuery, TResponse>
		where TQuery : IQuery<TResponse>
		where TResponse : class?
	{
	}
}