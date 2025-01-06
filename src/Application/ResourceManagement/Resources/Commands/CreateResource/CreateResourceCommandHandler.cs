using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.ResourceManagement.Resources.Commands.CreateResource;

internal class CreateResourceCommandHandler
    : ICommandHandler<CreateResourceCommand>
{
    public Task<Result> Handle(CreateResourceCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Success());
    }
}
