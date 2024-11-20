using AgendaManager.Domain.Authorization.Interfaces;
using AgendaManager.Domain.Authorization.Services;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Authorization.Services.PermissionManagers;

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
