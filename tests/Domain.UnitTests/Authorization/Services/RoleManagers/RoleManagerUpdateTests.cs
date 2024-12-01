using AgendaManager.Domain.Authorization;
using AgendaManager.Domain.Authorization.Errors;
using AgendaManager.Domain.Authorization.Interfaces;
using AgendaManager.Domain.Authorization.Services;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Authorization.Services.RoleManagers;

public class RoleManagerUpdateTests
{
    private readonly IRoleRepository _roleRepository;
    private readonly RoleManager _sut;

    public RoleManagerUpdateTests()
    {
        _roleRepository = Substitute.For<IRoleRepository>();
        _sut = new RoleManager(_roleRepository);
    }

    [Fact]
    public async Task Update_ShouldCallUpdateAsyncOnRoleRepository()
    {
        // Arrange
        var role = RoleFactory.CreateRole(isEditable: true);
        _roleRepository.ExistsByNameAsync(Arg.Any<Role>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(false));

        // Act
        var result = await _sut.UpdateRoleAsync(role, "newName", "newDescription", CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Update_ShouldReturnFailure_WhenRoleNameAlreadyExists()
    {
        // Arrange
        var role = RoleFactory.CreateRole(isEditable: true);
        _roleRepository.ExistsByNameAsync(Arg.Any<Role>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(true));

        // Act
        var result = await _sut.UpdateRoleAsync(role, "newName", "newDescription", CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Update_ShouldReturnFailure_WhenRoleIsNotEditable()
    {
        // Arrange
        var role = RoleFactory.CreateRole(isEditable: false);

        // Act
        var result = await _sut.UpdateRoleAsync(role, "newName", "newDescription", CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(RoleErrors.RoleIsNotEditable.FirstError());
    }
}
