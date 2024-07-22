using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace AgendaManager.Domain.UnitTests.Users.Services;

public class UserAuthorizationManagerTests
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly UserAuthorizationManager _sut;
    private readonly IUserRepository _userRepository;

    public UserAuthorizationManagerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _roleRepository = Substitute.For<IRoleRepository>();
        _permissionRepository = Substitute.For<IPermissionRepository>();
        _sut = new UserAuthorizationManager(_userRepository, _roleRepository, _permissionRepository);
    }

    [Fact]
    public async Task AddRoleToUserAsync_ShouldAddRoleToUser_WhenUserNotHaveRole()
    {
        // Arrange
        var user = UserFactory.CreateUserCarol();
        var role = RoleFactory.CreateRoleAdmin();
        _userRepository.GetByIdWithRolesAsync(user.Id).Returns(user);
        _roleRepository.GetByIdAsync(role.Id).Returns(role);

        // Act
        var result = await _sut.AddRoleToUserAsync(user.Id, role.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        user.Roles.Should().Contain(role);
        user.DomainEvents.Should().Contain(x => x is UserRoleAddedDomainEvent);
    }

    [Fact]
    public async Task AddRoleToUserAsync_ShouldReturnErrorUserNotFound_WhenUserNotFound()
    {
        // Arrange
        var user = UserFactory.CreateUserCarol();
        var role = RoleFactory.CreateRoleAdmin();
        var descriptionError = IdentityUserErrors.UserNotFound.FirstError()?.Description;
        _userRepository.GetByIdWithRolesAsync(user.Id).ReturnsNull();

        // Act
        var result = await _sut.AddRoleToUserAsync(user.Id, role.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
    }

    [Fact]
    public async Task AddRoleToUserAsync_ShouldReturnErrorRoleNotFound_WhenRoleNotFound()
    {
        // Arrange
        var user = UserFactory.CreateUserCarol();
        var role = RoleFactory.CreateRoleAdmin();
        var descriptionError = IdentityUserErrors.RoleNotFound.FirstError()?.Description;
        _userRepository.GetByIdWithRolesAsync(user.Id).Returns(user);
        _roleRepository.GetByIdAsync(role.Id).ReturnsNull();

        // Act
        var result = await _sut.AddRoleToUserAsync(user.Id, role.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
    }

    [Fact]
    public async Task AddRoleToUserAsync_ShouldReturnErrorRoleAlreadyExists_WhenUserAlreadyHaveRole()
    {
        // Arrange
        var user = UserFactory.CreateUserCarol();
        var role = RoleFactory.CreateRoleAdmin();
        var descriptionError = IdentityUserErrors.RoleAlreadyExists.FirstError()?.Description;
        _userRepository.GetByIdWithRolesAsync(user.Id).Returns(user);
        _roleRepository.GetByIdAsync(role.Id).Returns(role);

        // Act
        user.AddRole(role);
        var result = await _sut.AddRoleToUserAsync(user.Id, role.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
        user.Roles.Should().Contain(role);
    }

    [Fact]
    public async Task RemoveRoleFromUserAsync_ShouldRemoveRoleFromUser_WhenUserHaveRole()
    {
        // Arrange
        var user = UserFactory.CreateUserCarol();
        var role = RoleFactory.CreateRoleAdmin();
        _userRepository.GetByIdWithRolesAsync(user.Id).Returns(user);
        _roleRepository.GetByIdAsync(role.Id).Returns(role);

        // Act
        user.AddRole(role);
        var result = await _sut.RemoveRoleFromUserAsync(user.Id, role.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        user.Roles.Should().NotContain(role);
        user.DomainEvents.Should().Contain(x => x is UserRoleRemovedDomainEvent);
    }

    [Fact]
    public async Task RemoveRoleFromUserAsync_ShouldReturnErrorUserNotFound_WhenUserNotFound()
    {
        // Arrange
        var user = UserFactory.CreateUserCarol();
        var role = RoleFactory.CreateRoleAdmin();
        var descriptionError = IdentityUserErrors.UserNotFound.FirstError()?.Description;
        _userRepository.GetByIdWithRolesAsync(user.Id).ReturnsNull();

        // Act
        var result = await _sut.RemoveRoleFromUserAsync(user.Id, role.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
    }

    [Fact]
    public async Task RemoveRoleFromUserAsync_ShouldReturnErrorRoleNotFound_WhenRoleNotFound()
    {
        // Arrange
        var user = UserFactory.CreateUserCarol();
        var role = RoleFactory.CreateRoleAdmin();
        var descriptionError = IdentityUserErrors.RoleNotFound.FirstError()?.Description;
        _userRepository.GetByIdWithRolesAsync(user.Id).Returns(user);
        _roleRepository.GetByIdAsync(role.Id).ReturnsNull();

        // Act
        var result = await _sut.RemoveRoleFromUserAsync(user.Id, role.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
    }

    [Fact]
    public async Task
        RemoveRoleFromUserAsync_ShouldReturnErrorUserDoesNotHaveRoleAssigned_WhenUserDoesNotHaveRoleAssigned()
    {
        // Arrange
        var user = UserFactory.CreateUserCarol();
        var role = RoleFactory.CreateRoleAdmin();
        var descriptionError = IdentityUserErrors.UserDoesNotHaveRoleAssigned.FirstError()?.Description;
        _userRepository.GetByIdWithRolesAsync(user.Id).Returns(user);
        _roleRepository.GetByIdAsync(role.Id).Returns(role);

        // Act
        var result = await _sut.RemoveRoleFromUserAsync(user.Id, role.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
    }

    [Fact]
    public async Task AddPermissionToRole_ShouldAddPermissionToRole_WhenRoleNotHavePermission()
    {
        // Arrange
        var role = RoleFactory.CreateRoleAdmin();
        var permission = PermissionFactory.CreatePermissionUsersCreate();
        _roleRepository.GetByIdAsync(role.Id).Returns(role);
        _permissionRepository.GetByIdAsync(permission.Id).Returns(permission);

        // Act
        var result = await _sut.AddPermissionToRole(role.Id, permission.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        role.Permissions.Should().Contain(permission);
        role.DomainEvents.Should().Contain(x => x is RolePermissionAddedDomainEvent);
    }

    [Fact]
    public async Task AddPermissionToRole_ShouldReturnErrorRoleNotFound_WhenRoleNotFound()
    {
        // Arrange
        var role = RoleFactory.CreateRoleAdmin();
        var permission = PermissionFactory.CreatePermissionUsersCreate();
        var descriptionError = IdentityUserErrors.RoleNotFound.FirstError()?.Description;
        _roleRepository.GetByIdAsync(role.Id).ReturnsNull();
        _permissionRepository.GetByIdAsync(permission.Id).Returns(permission);

        // Act
        var result = await _sut.AddPermissionToRole(role.Id, permission.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
    }

    [Fact]
    public async Task AddPermissionToRole_ShouldReturnErrorPermissionNotFound_WhenPermissionNotFound()
    {
        // Arrange
        var role = RoleFactory.CreateRoleAdmin();
        var permission = PermissionFactory.CreatePermissionUsersCreate();
        var descriptionError = IdentityUserErrors.PermissionNotFound.FirstError()?.Description;
        _roleRepository.GetByIdAsync(role.Id).Returns(role);
        _permissionRepository.GetByIdAsync(permission.Id).ReturnsNull();

        // Act
        var result = await _sut.AddPermissionToRole(role.Id, permission.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
    }

    [Fact]
    public async Task AddPermissionToRole_ShouldReturnErrorRoleAlreadyHavePermission_WhenRoleAlreadyHavePermission()
    {
        // Arrange
        var role = RoleFactory.CreateRoleAdmin();
        var permission = PermissionFactory.CreatePermissionUsersCreate();
        var descriptionError = IdentityUserErrors.PermissionAlreadyExists.FirstError()?.Description;
        _roleRepository.GetByIdAsync(role.Id).Returns(role);
        _permissionRepository.GetByIdAsync(permission.Id).Returns(permission);

        // Act
        role.AddPermission(permission);
        var result = await _sut.AddPermissionToRole(role.Id, permission.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
    }

    [Fact]
    public async Task RemovePermissionFromRole_ShouldRemovePermissionFromRole_WhenRoleHavePermission()
    {
        // Arrange
        var role = RoleFactory.CreateRoleAdmin();
        var permission = PermissionFactory.CreatePermissionUsersCreate();
        _roleRepository.GetByIdAsync(role.Id).Returns(role);
        _permissionRepository.GetByIdAsync(permission.Id).Returns(permission);

        // Act
        role.AddPermission(permission);
        var result = await _sut.RemovePermissionFromRole(role.Id, permission.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        role.Permissions.Should().NotContain(permission);
        role.DomainEvents.Should().Contain(x => x is RolePermissionRemovedDomainEvent);
    }

    [Fact]
    public async Task RemovePermissionFromRole_ShouldReturnErrorRoleNotFound_WhenRoleNotFound()
    {
        // Arrange
        var role = RoleFactory.CreateRoleAdmin();
        var permission = PermissionFactory.CreatePermissionUsersCreate();
        var descriptionError = IdentityUserErrors.RoleNotFound.FirstError()?.Description;
        _roleRepository.GetByIdAsync(role.Id).ReturnsNull();
        _permissionRepository.GetByIdAsync(permission.Id).Returns(permission);

        // Act
        var result = await _sut.RemovePermissionFromRole(role.Id, permission.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
    }

    [Fact]
    public async Task RemovePermissionFromRole_ShouldReturnErrorPermissionNotFound_WhenPermissionNotFound()
    {
        // Arrange
        var role = RoleFactory.CreateRoleAdmin();
        var permission = PermissionFactory.CreatePermissionUsersCreate();
        var descriptionError = IdentityUserErrors.PermissionNotFound.FirstError()?.Description;
        _roleRepository.GetByIdAsync(role.Id).Returns(role);
        _permissionRepository.GetByIdAsync(permission.Id).ReturnsNull();

        // Act
        var result = await _sut.RemovePermissionFromRole(role.Id, permission.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
    }

    [Fact]
    public async Task
        RemovePermissionFromRole_ShouldReturnErrorRoleDontHavePermissionAssigned_WhenRoleDontHavePermission()
    {
        // Arrange
        var role = RoleFactory.CreateRoleAdmin();
        var permission = PermissionFactory.CreatePermissionUsersCreate();
        var descriptionError = IdentityUserErrors.RoleDoesNotHavePermissionAssigned.FirstError()?.Description;
        _roleRepository.GetByIdAsync(role.Id).Returns(role);
        _permissionRepository.GetByIdAsync(permission.Id).Returns(permission);

        // Act
        var result = await _sut.RemovePermissionFromRole(role.Id, permission.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be(descriptionError);
    }
}
