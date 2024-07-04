using AgendaManager.Domain.Common.Responses;
using MediatR;

namespace AgendaManager.Application.Common.Interfaces.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>, IBaseCommandQuery
{
}
