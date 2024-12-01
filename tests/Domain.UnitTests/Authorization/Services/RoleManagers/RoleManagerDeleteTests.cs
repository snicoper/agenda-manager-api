using AgendaManager.Domain.Authorization.Interfaces;
using AgendaManager.Domain.Authorization.Services;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace AgendaManager.Domain.UnitTests.Authorization.Services.RoleManagers;

public class RoleManagerDeleteTests
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly RoleManager _sut;

    public RoleManagerDeleteTests()
    {
        _roleRepository = Substitute.For<IRoleRepository>();
        _userRepository = Substitute.For<IUserRepository>();
        _sut = new RoleManager(_roleRepository, _userRepository);
    }

    [Fact]
    public async Task Delete_ShouldSuccess_WhenDeleteRole()
    {
        // Arrange
        var role = RoleFactory.CreateRole();
        _roleRepository.GetByIdAsync(role.Id)
            .Returns(role);

        _userRepository.HasAnyUserWithRole(role.Id)
            .Returns(false);

        // Act
        var result = await _sut.DeleteRoleAsync(role.Id, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_ShouldFailure_WhenRoleNotFound()
    {
        // Arrange
        var role = RoleFactory.CreateRole();
        _roleRepository.GetByIdAsync(role.Id)
            .ReturnsNull();

        // Act
        var result = await _sut.DeleteRoleAsync(role.Id, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_ShouldFailure_WhenRoleHasUsers()
    {
        // Arrange
        var role = RoleFactory.CreateRole();
        _roleRepository.GetByIdAsync(role.Id)
            .Returns(role);
        _userRepository.HasAnyUserWithRole(role.Id)
            .Returns(true);

        // Act
        var result = await _sut.DeleteRoleAsync(role.Id, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
    }
}
