using MediatR;

namespace Services.Shared.CQRS
{
	public interface ICommand<out TResponse>
		: IRequest<TResponse>
		where TResponse : notnull
	{
	}

	public interface ICommandHandler<in TCommand, TResponse>
		: IRequestHandler<TCommand, TResponse>
		where TCommand : ICommand<TResponse>
		where TResponse : notnull
	{
	}
}