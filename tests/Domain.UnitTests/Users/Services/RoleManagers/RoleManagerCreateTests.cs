using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.Exceptions;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Users.Services.RoleManagers;

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
        var role = RoleFactory.CreateRoleAdmin();

        // Act
        var roleResult = await _sut.CreateRoleAsync(role.Id, role.Name, role.Editable, Arg.Any<CancellationToken>());

        // Assert
        roleResult.Should().BeOfType<Result<Role>>();
        roleResult.IsSuccess.Should().BeTrue();
        roleResult.ResultType.Should().Be(ResultType.Created);
        roleResult.Value!.Id.Should().Be(role.Id);
        roleResult.Value.Name.Should().Be(role.Name);
        roleResult.Value.Editable.Should().Be(role.Editable);
        roleResult.Value.DomainEvents.Should().Contain(x => x is RoleCreatedDomainEvent);
    }

    [Fact]
    public async Task CreateRoleAsync_ShouldReturnResultFailure_WhenNameAlreadyExists()
    {
        // Arrange
        var role = RoleFactory.CreateRoleAdmin();

        // Act
        _roleRepository.NameExistsAsync(Arg.Any<Role>(), CancellationToken.None).Returns(true);
        var roleResult = await _sut.CreateRoleAsync(role.Id, role.Name, role.Editable);

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

        // Assert
        await Assert.ThrowsAsync<RoleDomainException>(() => _sut.CreateRoleAsync(RoleId.Create(), roleName, true));
    }
}
