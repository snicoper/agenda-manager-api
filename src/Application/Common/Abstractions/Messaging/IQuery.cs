using AgendaManager.Domain.Common.Abstractions;
using MediatR;

namespace AgendaManager.Application.Common.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
