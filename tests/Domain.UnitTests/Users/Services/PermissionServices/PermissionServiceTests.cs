﻿using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Users.Services.PermissionServices;

public class PermissionServiceTests
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly PermissionService _sut;

    public PermissionServiceTests()
    {
        _permissionRepository = Substitute.For<IPermissionRepository>();
        _sut = new PermissionService(_permissionRepository);
    }

    [Fact]
    public async Task CreateAsync_Should_ReturnResultSuccess_WhenPermissionIsCreated()
    {
        // Arrange
        var permission = PermissionFactory.CreatePermissionUsersCreate();

        // Act
        var permissionResult = await _sut.CreateAsync(permission.Id, permission.Name, Arg.Any<CancellationToken>());

        // Assert
        permissionResult.Should().BeOfType<Result<Permission>>();
        permissionResult.IsSuccess.Should().BeTrue();
        permissionResult.ResultType.Should().Be(ResultType.Created);
        permissionResult.Value!.Id.Should().Be(permission.Id);
        permissionResult.Value.Name.Should().Be(permission.Name);
        permissionResult.Value.DomainEvents.Should().Contain(x => x is PermissionCreatedDomainEvent);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnResultFailure_WhenNameAlreadyExists()
    {
        // Arrange
        var permission = PermissionFactory.CreatePermissionUsersCreate();

        // Act
        _permissionRepository.NameExistsAsync(Arg.Any<Permission>(), CancellationToken.None).Returns(true);
        var permissionResult = await _sut.CreateAsync(permission.Id, permission.Name);

        // Assert
        permissionResult.Should().BeOfType<Result<Permission>>();
        permissionResult.IsSuccess.Should().BeFalse();
        permissionResult.ResultType.Should().Be(ResultType.Validation);
        permissionResult.Error?.FirstError()?.Description.Should().Be("Permission name already exists.");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnResultFailure_WhenNameExceedsLength()
    {
        // Arrange
        var permissionName = new string('a', 101);
        var permission = PermissionFactory.CreatePermissionUsersCreate(name: permissionName);

        // Act
        var permissionResult = await _sut.CreateAsync(permission.Id, permission.Name);

        // Assert
        permissionResult.Should().BeOfType<Result<Permission>>();
        permissionResult.IsSuccess.Should().BeFalse();
        permissionResult.ResultType.Should().Be(ResultType.Validation);
        permissionResult.Error?.FirstError()?.Description.Should().Be("Permission name exceeds length.");
    }
}
