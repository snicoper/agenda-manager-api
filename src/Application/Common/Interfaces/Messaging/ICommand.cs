using AgendaManager.Domain.Common.Abstractions;
using MediatR;

namespace AgendaManager.Application.Common.Interfaces.Messaging;

public interface ICommand : IRequest<Result>, IBaseCommand
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
{
}

public interface IBaseCommand
{
}
