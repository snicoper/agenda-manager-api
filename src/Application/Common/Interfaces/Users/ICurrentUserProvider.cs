namespace AgendaManager.Application.Common.Interfaces.Users;

public interface ICurrentUserProvider
{
    Guid Id { get; }

    IEnumerable<Guid> Roles { get; }
}
