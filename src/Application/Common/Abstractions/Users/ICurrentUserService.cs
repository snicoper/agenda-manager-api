namespace AgendaManager.Application.Common.Abstractions.Users;

public interface ICurrentUserService
{
    Guid Id { get; }

    IEnumerable<Guid> Roles { get; }
}
