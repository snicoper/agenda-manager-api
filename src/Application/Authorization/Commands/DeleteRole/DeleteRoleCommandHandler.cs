using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Authorization.Commands.DeleteRole;

internal class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand>
{
    public Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
