namespace AgendaManager.Application.Common.Interfaces.Users;

public interface ICurrentUserService
{
    Guid Id { get; }

    IEnumerable<Guid> Roles { get; }
}
