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

public class AuthorizationManagerTests
{
    private readonly User _user;
    private readonly Role _role;
    private readonly Permission _permission;

    private readonly IPermissionRepository _permissionRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly AuthorizationManager _sut;

    public AuthorizationManagerTests()
    {
        _user = UserFactory.CreateUserAlice();
        _role = RoleFactory.CreateRole();
        _permission = PermissionFactory.CreatePermissionUsersCreate();

        _userRepository = Substitute.For<IUserRepository>();
        _roleRepository = Substitute.For<IRoleRepository>();
        _permissionRepository = Substitute.For<IPermissionRepository>();

        _sut = new AuthorizationManager(_userRepository, _roleRepository, _permissionRepository);
    }

    [Fact]
    public async Task AddRoleToUserAsync_ShouldAddRoleToUser_WhenUserNotHaveRole()
    {
        // Arrange
        _userRepository.GetByIdWithRolesAsync(_user.Id).Returns(_user);
        _roleRepository.GetByIdAsync(_role.Id).Returns(_role);

        // Act
        var result = await _sut.AddRoleToUserAsync(_user.Id, _role.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ResultType.Should().Be(ResultType.Succeeded);
    }

    [Fact]
    public async Task AddRoleToUserAsync_ShouldReturnErrorUserNotFound_WhenUserNotFound()
    {
        // Arrange
        var descriptionError = UserErrors.UserNotFound.FirstError()?.Description;
        _userRepository.GetByIdWithRolesAsync(_user.Id).ReturnsNull();

        // Act
        var result = await _sut.AddRoleToUserAsync(_user.Id, _role.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
        result.ResultType.Should().Be(ResultType.NotFound);
    }

    [Fact]
    public async Task AddRoleToUserAsync_ShouldReturnErrorRoleNotFound_WhenRoleNotFound()
    {
        // Arrange
        var descriptionError = RoleErrors.RoleNotFound.FirstError()?.Description;
        _userRepository.GetByIdWithRolesAsync(_user.Id).Returns(_user);
        _roleRepository.GetByIdAsync(_role.Id).ReturnsNull();

        // Act
        var result = await _sut.AddRoleToUserAsync(_user.Id, _role.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
        result.ResultType.Should().Be(ResultType.NotFound);
    }

    [Fact]
    public async Task AddRoleToUserAsync_ShouldReturnErrorRoleAlreadyExists_WhenUserAlreadyHaveRole()
    {
        // Arrange
        var descriptionError = RoleErrors.RoleAlreadyExists.FirstError()?.Description;
        _userRepository.GetByIdWithRolesAsync(_user.Id).Returns(_user);
        _roleRepository.GetByIdAsync(_role.Id).Returns(_role);

        // Act
        _user.AddRole(_role);
        var result = await _sut.AddRoleToUserAsync(_user.Id, _role.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
        result.ResultType.Should().Be(ResultType.Conflict);
    }

    [Fact]
    public async Task RemoveRoleFromUserAsync_ShouldRemoveRoleFromUser_WhenUserHaveRole()
    {
        // Arrange
        _userRepository.GetByIdWithRolesAsync(_user.Id).Returns(_user);
        _roleRepository.GetByIdAsync(_role.Id).Returns(_role);

        // Act
        _user.AddRole(_role);
        var result = await _sut.RemoveRoleFromUserAsync(_user.Id, _role.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ResultType.Should().Be(ResultType.Succeeded);
    }

    [Fact]
    public async Task RemoveRoleFromUserAsync_ShouldReturnErrorUserNotFound_WhenUserNotFound()
    {
        // Arrange
        var descriptionError = UserErrors.UserNotFound.FirstError()?.Description;
        _userRepository.GetByIdWithRolesAsync(_user.Id).ReturnsNull();

        // Act
        var result = await _sut.RemoveRoleFromUserAsync(_user.Id, _role.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
        result.ResultType.Should().Be(ResultType.NotFound);
    }

    [Fact]
    public async Task RemoveRoleFromUserAsync_ShouldReturnErrorRoleNotFound_WhenRoleNotFound()
    {
        // Arrange
        var descriptionError = RoleErrors.RoleNotFound.FirstError()?.Description;
        _userRepository.GetByIdWithRolesAsync(_user.Id).Returns(_user);
        _roleRepository.GetByIdAsync(_role.Id).ReturnsNull();

        // Act
        var result = await _sut.RemoveRoleFromUserAsync(_user.Id, _role.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
        result.ResultType.Should().Be(ResultType.NotFound);
    }

    [Fact]
    public async Task
        RemoveRoleFromUserAsync_ShouldReturnErrorUserDoesNotHaveRoleAssigned_WhenUserDoesNotHaveRoleAssigned()
    {
        // Arrange
        var descriptionError = UserErrors.UserDoesNotHaveRoleAssigned.FirstError()?.Description;
        _userRepository.GetByIdWithRolesAsync(_user.Id).Returns(_user);
        _roleRepository.GetByIdAsync(_role.Id).Returns(_role);

        // Act
        var result = await _sut.RemoveRoleFromUserAsync(_user.Id, _role.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
        result.ResultType.Should().Be(ResultType.Conflict);
    }

    [Fact]
    public async Task AddPermissionToRole_ShouldAddPermissionToRole_WhenRoleNotHavePermission()
    {
        // Arrange
        _roleRepository.GetByIdAsync(_role.Id).Returns(_role);
        _permissionRepository.GetByIdAsync(_permission.Id).Returns(_permission);

        // Act
        var result = await _sut.AddPermissionToRole(_role.Id, _permission.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ResultType.Should().Be(ResultType.Succeeded);
    }

    [Fact]
    public async Task AddPermissionToRole_ShouldReturnErrorRoleNotFound_WhenRoleNotFound()
    {
        // Arrange
        var descriptionError = RoleErrors.RoleNotFound.FirstError()?.Description;
        _roleRepository.GetByIdAsync(_role.Id).ReturnsNull();
        _permissionRepository.GetByIdAsync(_permission.Id).Returns(_permission);

        // Act
        var result = await _sut.AddPermissionToRole(_role.Id, _permission.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
        result.ResultType.Should().Be(ResultType.NotFound);
    }

    [Fact]
    public async Task AddPermissionToRole_ShouldReturnErrorPermissionNotFound_WhenPermissionNotFound()
    {
        // Arrange
        var descriptionError = PermissionErrors.PermissionNotFound.FirstError()?.Description;
        _roleRepository.GetByIdAsync(_role.Id).Returns(_role);
        _permissionRepository.GetByIdAsync(_permission.Id).ReturnsNull();

        // Act
        var result = await _sut.AddPermissionToRole(_role.Id, _permission.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
        result.ResultType.Should().Be(ResultType.NotFound);
    }

    [Fact]
    public async Task AddPermissionToRole_ShouldReturnErrorRoleAlreadyHavePermission_WhenRoleAlreadyHavePermission()
    {
        // Arrange
        var descriptionError = PermissionErrors.PermissionAlreadyExists.FirstError()?.Description;
        _roleRepository.GetByIdAsync(_role.Id).Returns(_role);
        _permissionRepository.GetByIdAsync(_permission.Id).Returns(_permission);

        // Act
        _role.AddPermission(_permission);
        var result = await _sut.AddPermissionToRole(_role.Id, _permission.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
        result.ResultType.Should().Be(ResultType.Conflict);
    }

    [Fact]
    public async Task RemovePermissionFromRole_ShouldRemovePermissionFromRole_WhenRoleHavePermission()
    {
        // Arrange
        _roleRepository.GetByIdAsync(_role.Id).Returns(_role);
        _permissionRepository.GetByIdAsync(_permission.Id).Returns(_permission);

        // Act
        _role.AddPermission(_permission);
        var result = await _sut.RemovePermissionFromRole(_role.Id, _permission.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ResultType.Should().Be(ResultType.Succeeded);
    }

    [Fact]
    public async Task RemovePermissionFromRole_ShouldReturnErrorRoleNotFound_WhenRoleNotFound()
    {
        // Arrange
        var descriptionError = RoleErrors.RoleNotFound.FirstError()?.Description;
        _roleRepository.GetByIdAsync(_role.Id).ReturnsNull();
        _permissionRepository.GetByIdAsync(_permission.Id).Returns(_permission);

        // Act
        var result = await _sut.RemovePermissionFromRole(_role.Id, _permission.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
        result.ResultType.Should().Be(ResultType.NotFound);
    }

    [Fact]
    public async Task RemovePermissionFromRole_ShouldReturnErrorPermissionNotFound_WhenPermissionNotFound()
    {
        // Arrange
        var descriptionError = PermissionErrors.PermissionNotFound.FirstError()?.Description;
        _roleRepository.GetByIdAsync(_role.Id).Returns(_role);
        _permissionRepository.GetByIdAsync(_permission.Id).ReturnsNull();

        // Act
        var result = await _sut.RemovePermissionFromRole(_role.Id, _permission.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
        result.ResultType.Should().Be(ResultType.NotFound);
    }

    [Fact]
    public async Task
        RemovePermissionFromRole_ShouldReturnErrorRoleDontHavePermissionAssigned_WhenRoleDontHavePermission()
    {
        // Arrange
        var descriptionError = RoleErrors.RoleDoesNotHavePermissionAssigned.FirstError()?.Description;
        _roleRepository.GetByIdAsync(_role.Id).Returns(_role);
        _permissionRepository.GetByIdAsync(_permission.Id).Returns(_permission);

        // Act
        var result = await _sut.RemovePermissionFromRole(_role.Id, _permission.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
        result.ResultType.Should().Be(ResultType.Conflict);
    }
}
