using AgendaManager.Domain.Common.Abstractions;
using MediatR;

namespace AgendaManager.Application.Common.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
