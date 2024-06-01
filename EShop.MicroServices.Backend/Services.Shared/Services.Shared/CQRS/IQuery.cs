using MediatR;

namespace Services.Shared.CQRS
{
	public interface IQuery<TResponse>
		: IRequest<TResponse>
		where TResponse : notnull
	{
	}

	public interface IQueryHandler<in TQuery, TResponse>
		: IRequestHandler<TQuery, TResponse>
		where TQuery : IQuery<TResponse>
		where TResponse : notnull
	{
	}
}