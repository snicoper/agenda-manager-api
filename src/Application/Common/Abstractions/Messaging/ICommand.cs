using AgendaManager.Domain.Common.Abstractions;
using MediatR;

namespace AgendaManager.Application.Common.Abstractions.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
