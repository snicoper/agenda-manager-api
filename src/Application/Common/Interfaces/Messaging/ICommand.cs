using AgendaManager.Domain.Common.Responses;
using MediatR;

namespace AgendaManager.Application.Common.Interfaces.Messaging;

public interface ICommand : IRequest<Result>, IAppBaseRequest
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IAppBaseRequest
{
}
