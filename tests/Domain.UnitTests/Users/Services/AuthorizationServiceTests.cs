using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace AgendaManager.Domain.UnitTests.Users.Services;

public class AuthorizationServiceTests
{
    private readonly User _user;
    private readonly Role _role;
    private readonly Permission _permission;

    private readonly IPermissionRepository _permissionRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly AuthorizationService _sut;

    public AuthorizationServiceTests()
    {
        _user = UserFactory.CreateUserAlice();
        _role = RoleFactory.CreateRole();
        _permission = PermissionFactory.CreatePermissionUsersCreate();

        _userRepository = Substitute.For<IUserRepository>();
        _roleRepository = Substitute.For<IRoleRepository>();
        _permissionRepository = Substitute.For<IPermissionRepository>();

        _sut = new AuthorizationService(_userRepository, _roleRepository, _permissionRepository);
    }

    [Fact]
    public async Task AddRoleToUserAsync_ShouldSuccess()
    {
        // Arrange
        _userRepository.GetByIdWithRolesAsync(_user.Id).Returns(_user);
        _roleRepository.GetByIdAsync(_role.Id).Returns(_role);

        // Act
        var result = await _sut.AddRoleToUserAsync(_user.Id, _role.Id, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ResultType.Should().Be(ResultType.Succeeded);
    }

    [Fact]
    public async Task AddRoleToUser_ShouldFailure_WhenUserNotFound()
    {
        // Arrange
        var descriptionError = UserErrors.UserNotFound.FirstError()?.Description;
        _userRepository.GetByIdWithRolesAsync(_user.Id).ReturnsNull();

        // Act
        var result = await _sut.AddRoleToUserAsync(_user.Id, _role.Id, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
        result.ResultType.Should().Be(ResultType.NotFound);
    }

    [Fact]
    public async Task AddRoleToUser_ShouldFailure_WhenRoleNotFound()
    {
        // Arrange
        var descriptionError = RoleErrors.RoleNotFound.FirstError()?.Description;
        _userRepository.GetByIdWithRolesAsync(_user.Id).Returns(_user);
        _roleRepository.GetByIdAsync(_role.Id).ReturnsNull();

        // Act
        var result = await _sut.AddRoleToUserAsync(_user.Id, _role.Id, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
        result.ResultType.Should().Be(ResultType.NotFound);
    }

    [Fact]
    public async Task AddRoleToUser_ShouldSuccess_WhenUserAlreadyHaveRole()
    {
        // Arrange
        _userRepository.GetByIdWithRolesAsync(_user.Id).Returns(_user);
        _roleRepository.GetByIdAsync(_role.Id).Returns(_role);

        // Act
        _user.AddRole(_role);
        var result = await _sut.AddRoleToUserAsync(_user.Id, _role.Id, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task RemoveRoleFromUser_ShouldSuccess()
    {
        // Arrange
        _userRepository.GetByIdWithRolesAsync(_user.Id).Returns(_user);
        _roleRepository.GetByIdAsync(_role.Id).Returns(_role);

        // Act
        _user.AddRole(_role);
        var result = await _sut.RemoveRoleFromUserAsync(_user.Id, _role.Id, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ResultType.Should().Be(ResultType.Succeeded);
    }

    [Fact]
    public async Task RemoveRoleFromUser_ShouldSuccess_WhenUserNotFound()
    {
        // Arrange
        var descriptionError = UserErrors.UserNotFound.FirstError()?.Description;
        _userRepository.GetByIdWithRolesAsync(_user.Id).ReturnsNull();

        // Act
        var result = await _sut.RemoveRoleFromUserAsync(_user.Id, _role.Id, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
        result.ResultType.Should().Be(ResultType.NotFound);
    }

    [Fact]
    public async Task RemoveRoleFromUser_ShouldFailure_WhenRoleNotFound()
    {
        // Arrange
        var descriptionError = RoleErrors.RoleNotFound.FirstError()?.Description;
        _userRepository.GetByIdWithRolesAsync(_user.Id).Returns(_user);
        _roleRepository.GetByIdAsync(_role.Id).ReturnsNull();

        // Act
        var result = await _sut.RemoveRoleFromUserAsync(_user.Id, _role.Id, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
        result.ResultType.Should().Be(ResultType.NotFound);
    }

    [Fact]
    public async Task RemoveRoleFromUser_ShouldSuccess_WhenUserDoesNotHaveRoleAssigned()
    {
        // Arrange
        _userRepository.GetByIdWithRolesAsync(_user.Id).Returns(_user);
        _roleRepository.GetByIdAsync(_role.Id).Returns(_role);

        // Act
        var result = await _sut.RemoveRoleFromUserAsync(_user.Id, _role.Id, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task AddPermissionToRole_ShouldSuccess_WhenRoleNotHavePermission()
    {
        // Arrange
        _roleRepository.GetByIdAsync(_role.Id).Returns(_role);
        _permissionRepository.GetByIdAsync(_permission.Id).Returns(_permission);

        // Act
        var result = await _sut.AddPermissionToRole(_role.Id, _permission.Id, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ResultType.Should().Be(ResultType.Succeeded);
    }

    [Fact]
    public async Task AddPermissionToRole_ShouldFailure_WhenRoleNotFound()
    {
        // Arrange
        var descriptionError = RoleErrors.RoleNotFound.FirstError()?.Description;
        _roleRepository.GetByIdAsync(_role.Id).ReturnsNull();
        _permissionRepository.GetByIdAsync(_permission.Id).Returns(_permission);

        // Act
        var result = await _sut.AddPermissionToRole(_role.Id, _permission.Id, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
        result.ResultType.Should().Be(ResultType.NotFound);
    }

    [Fact]
    public async Task AddPermissionToRole_ShouldFailure_WhenPermissionNotFound()
    {
        // Arrange
        var descriptionError = PermissionErrors.PermissionNotFound.FirstError()?.Description;
        _roleRepository.GetByIdAsync(_role.Id).Returns(_role);
        _permissionRepository.GetByIdAsync(_permission.Id).ReturnsNull();

        // Act
        var result = await _sut.AddPermissionToRole(_role.Id, _permission.Id, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
        result.ResultType.Should().Be(ResultType.NotFound);
    }

    [Fact]
    public async Task AddPermissionToRole_ShouldSuccess_WhenRoleAlreadyHavePermission()
    {
        // Arrange
        _roleRepository.GetByIdAsync(_role.Id).Returns(_role);
        _permissionRepository.GetByIdAsync(_permission.Id).Returns(_permission);

        // Act
        _role.AddPermission(_permission);
        var result = await _sut.AddPermissionToRole(_role.Id, _permission.Id, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task RemovePermissionFromRole_ShouldSuccess_WhenRoleHavePermission()
    {
        // Arrange
        _roleRepository.GetByIdAsync(_role.Id).Returns(_role);
        _permissionRepository.GetByIdAsync(_permission.Id).Returns(_permission);

        // Act
        _role.AddPermission(_permission);
        var result = await _sut.RemovePermissionFromRole(_role.Id, _permission.Id, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ResultType.Should().Be(ResultType.Succeeded);
    }

    [Fact]
    public async Task RemovePermissionFromRole_ShouldFailure_WhenRoleNotFound()
    {
        // Arrange
        var descriptionError = RoleErrors.RoleNotFound.FirstError()?.Description;
        _roleRepository.GetByIdAsync(_role.Id).ReturnsNull();
        _permissionRepository.GetByIdAsync(_permission.Id).Returns(_permission);

        // Act
        var result = await _sut.RemovePermissionFromRole(_role.Id, _permission.Id, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
        result.ResultType.Should().Be(ResultType.NotFound);
    }

    [Fact]
    public async Task RemovePermissionFromRole_ShouldFailure_WhenPermissionNotFound()
    {
        // Arrange
        var descriptionError = PermissionErrors.PermissionNotFound.FirstError()?.Description;
        _roleRepository.GetByIdAsync(_role.Id).Returns(_role);
        _permissionRepository.GetByIdAsync(_permission.Id).ReturnsNull();

        // Act
        var result = await _sut.RemovePermissionFromRole(_role.Id, _permission.Id, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
        result.ResultType.Should().Be(ResultType.NotFound);
    }

    [Fact]
    public async Task RemovePermissionFromRole_ShouldSuccess_WhenRoleDontHavePermission()
    {
        // Arrange
        _roleRepository.GetByIdAsync(_role.Id).Returns(_role);
        _permissionRepository.GetByIdAsync(_permission.Id).Returns(_permission);

        // Act
        var result = await _sut.RemovePermissionFromRole(_role.Id, _permission.Id, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}
