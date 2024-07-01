using AgendaManager.Domain.Common.Abstractions;
using MediatR;

namespace AgendaManager.Application.Common.Interfaces.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
