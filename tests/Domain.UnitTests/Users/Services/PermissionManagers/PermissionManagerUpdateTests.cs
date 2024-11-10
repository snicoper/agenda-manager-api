using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Users.Services.PermissionManagers;

public class PermissionManagerUpdateTests
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly PermissionManager _sut;

    public PermissionManagerUpdateTests()
    {
        _permissionRepository = Substitute.For<IPermissionRepository>();
        _sut = new PermissionManager(_permissionRepository);
    }
}
