using AgendaManager.Domain.Authorization;
using AgendaManager.Domain.Authorization.Events;
using AgendaManager.Domain.Authorization.Exceptions;
using AgendaManager.Domain.Authorization.Interfaces;
using AgendaManager.Domain.Authorization.Services;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Authorization.Services.RoleManagers;

public class RoleManagerCreateTests
{
    private readonly IRoleRepository _roleRepository;
    private readonly RoleManager _sut;

    public RoleManagerCreateTests()
    {
        _roleRepository = Substitute.For<IRoleRepository>();
        _sut = new RoleManager(_roleRepository);
    }

    [Fact]
    public async Task CreateRoleAsync_Should_ReturnResultSuccess_WhenRoleIsCreated()
    {
        // Arrange
        var role = RoleFactory.CreateRole();

        // Act
        var roleResult = await _sut.CreateRoleAsync(
            role.Id,
            role.Name,
            role.Description,
            role.IsEditable,
            Arg.Any<CancellationToken>());

        // Assert
        roleResult.Should().BeOfType<Result<Role>>();
        roleResult.IsSuccess.Should().BeTrue();
        roleResult.ResultType.Should().Be(ResultType.Created);
        roleResult.Value!.Id.Should().Be(role.Id);
        roleResult.Value.Name.Should().Be(role.Name);
        roleResult.Value.IsEditable.Should().Be(role.IsEditable);
        roleResult.Value.DomainEvents.Should().Contain(x => x is RoleCreatedDomainEvent);
    }

    [Fact]
    public async Task CreateRoleAsync_ShouldReturnResultFailure_WhenNameAlreadyExists()
    {
        // Arrange
        var role = RoleFactory.CreateRole();

        // Act
        _roleRepository.ExistsByNameAsync(Arg.Any<Role>(), CancellationToken.None).Returns(true);
        var roleResult = await _sut.CreateRoleAsync(role.Id, role.Name, role.Description, role.IsEditable);

        // Assert
        roleResult.Should().BeOfType<Result<Role>>();
        roleResult.IsSuccess.Should().BeFalse();
        roleResult.ResultType.Should().Be(ResultType.Validation);
        roleResult.Error?.FirstError()?.Description.Should().Be("Role name already exists.");
    }

    [Fact]
    public async Task CreateRoleAsync_ShouldThrowRoleDomainException_WhenNameExceedsLength()
    {
        // Arrange
        var roleName = new string('a', 101);
        var roleDescription = "Description";

        // Assert
        await Assert.ThrowsAsync<RoleDomainException>(
            () => _sut.CreateRoleAsync(RoleId.Create(), roleName, roleDescription, true));
    }
}
