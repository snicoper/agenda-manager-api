using AgendaManager.Domain.Authorization.Entities;
using AgendaManager.Domain.Authorization.Exceptions;
using AgendaManager.Domain.Authorization.Interfaces;
using AgendaManager.Domain.Authorization.Services;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Authorization.Services.PermissionManagers;

public class PermissionManagerCreateTests
{
    private readonly Permission _permission;

    private readonly IPermissionRepository _permissionRepository;
    private readonly PermissionManager _sut;

    public PermissionManagerCreateTests()
    {
        _permissionRepository = Substitute.For<IPermissionRepository>();
        _sut = new PermissionManager(_permissionRepository);

        _permission = PermissionFactory.CreatePermissionUsersCreate();
    }

    [Fact]
    public async Task CreatePermissionAsync_Should_ReturnResultSuccess_WhenPermissionIsCreated()
    {
        // Act
        var permissionResult = await _sut.CreatePermissionAsync(
            _permission.Id,
            _permission.Name,
            Arg.Any<CancellationToken>());

        // Assert
        permissionResult.Should().BeOfType<Result<Permission>>();
        permissionResult.IsSuccess.Should().BeTrue();
        permissionResult.ResultType.Should().Be(ResultType.Created);
        permissionResult.Value!.Id.Should().Be(_permission.Id);
        permissionResult.Value.Name.Should().Be(_permission.Name);
        permissionResult.Value.DomainEvents.Should().Contain(x => x is PermissionCreatedDomainEvent);
    }

    [Fact]
    public async Task CreatePermissionAsync_ShouldReturnResultFailure_WhenNameAlreadyExists()
    {
        // Act
        _permissionRepository.NameExistsAsync(Arg.Any<Permission>(), CancellationToken.None).Returns(true);
        var permissionResult = await _sut.CreatePermissionAsync(_permission.Id, _permission.Name);

        // Assert
        permissionResult.Should().BeOfType<Result<Permission>>();
        permissionResult.IsSuccess.Should().BeFalse();
        permissionResult.ResultType.Should().Be(ResultType.Validation);
        permissionResult.Error?.FirstError()?.Description.Should().Be("Permission name already exists.");
    }

    [Fact]
    public async Task CreatePermissionAsync_ShouldThrowPermissionDomainException_WhenNameExceedsLength()
    {
        // Arrange
        var permissionName = new string('a', 101);

        // Assert
        await Assert.ThrowsAsync<PermissionDomainException>(
            () => _sut.CreatePermissionAsync(PermissionId.Create(), permissionName));
    }
}
