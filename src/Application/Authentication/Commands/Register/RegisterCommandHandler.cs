using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Authentication.Commands.Register;

internal class RegisterCommandHandler : ICommandHandler<RegisterCommand, RegisterCommandResponse>
{
    public Task<Result<RegisterCommandResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Success(new RegisterCommandResponse(Guid.NewGuid().ToString())));
    }
}
