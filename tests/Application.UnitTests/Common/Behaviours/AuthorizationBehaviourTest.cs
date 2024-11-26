using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Behaviours;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Users.Models;
using AgendaManager.Application.Users.Services;
using AgendaManager.Domain.Authorization.Constants;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Application.UnitTests.Common.Behaviours;

public class AuthorizationBehaviourTest
{
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly AuthorizationBehaviour<IAppBaseRequest, Result> _sut;

    public AuthorizationBehaviourTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();

        _sut = new AuthorizationBehaviour<IAppBaseRequest, Result>(_currentUserProvider);
    }

    [Fact]
    public async Task AuthorizationBehaviour_ShouldReturnSuccess_WhenNotRequiredRolesOrPermissions()
    {
        // Arrange
        TestRequest request = new();

        // Act
        var result = await _sut.Handle(request, () => Task.FromResult(Result.Success()), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error?.HasErrors.Should().BeFalse();
    }

    [Fact]
    public async Task AuthorizationBehaviour_ShouldReturnSuccess_WhenUserIsAuthenticated()
    {
        // Arrange
        List<string> roles = [SystemRoles.Administrator, SystemRoles.Employee, SystemRoles.Customer];
        List<string> permissions = [SystemPermissions.Users.Delete];

        TestAdminRequest request = new();
        _currentUserProvider.IsAuthenticated.Returns(true);
        _currentUserProvider
            .GetCurrentUser()
            .Returns(new CurrentUser(UserId.From(Guid.NewGuid()), roles, permissions));

        // Act
        var result = await _sut.Handle(request, () => Task.FromResult(Result.Success()), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error?.HasErrors.Should().BeFalse();
    }

    [Fact]
    public async Task AuthorizationBehaviour_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
    {
        // Arrange;
        TestAdminRequest request = new();
        _currentUserProvider.IsAuthenticated.Returns(false);

        // Act
        var result = await _sut
            .Handle(request, () => Task.FromResult(Result.Success()), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ResultType.Should().Be(ResultType.Unauthorized);
        result.Error?.HasErrors.Should().BeTrue();
        result.Error?.FirstError()?.Code.Should().Be("401");
        result.Error?.FirstError()?.Description.Should().Contain("User is Unauthorized from taking this action");
    }

    [Fact]
    public async Task AuthorizationBehaviour_ShouldReturnUnauthorized_WhenUserIsDefault()
    {
        // Arrange;
        TestAdminRequest request = new();
        var defaultCurrentUser = new CurrentUser(UserId.From(Guid.Empty), new List<string>(), new List<string>());
        _currentUserProvider.IsAuthenticated.Returns(true);
        _currentUserProvider.GetCurrentUser().Returns(defaultCurrentUser);

        // Act
        var result = await _sut
            .Handle(request, () => Task.FromResult(Result.Success()), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ResultType.Should().Be(ResultType.Unauthorized);
        result.Error?.HasErrors.Should().BeTrue();
        result.Error?.FirstError()?.Code.Should().Be("401");
        result.Error?.FirstError()?.Description.Should().Contain("User is Unauthorized from taking this action");
    }

    [Fact]
    public async Task AuthorizationBehaviour_ShouldReturnFailure_WhenMissingRequiredRoles()
    {
        // Arrange
        List<string> roles = [SystemRoles.Employee, SystemRoles.Customer];
        List<string> permissions = [SystemPermissions.Users.Delete];

        TestAdminRequest request = new();
        _currentUserProvider.IsAuthenticated.Returns(true);
        _currentUserProvider
            .GetCurrentUser()
            .Returns(new CurrentUser(UserId.From(Guid.NewGuid()), roles, permissions));

        // Act
        var result = await _sut.Handle(request, () => Task.FromResult(Result.Success()), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ResultType.Should().Be(ResultType.Unauthorized);
        result.Error?.HasErrors.Should().BeTrue();
        result.Error?.FirstError()?.Code.Should().Be("401");
        result.Error?.FirstError()?.Description.Should().Contain("User is Unauthorized from taking this action");
    }

    [Fact]
    public async Task AuthorizationBehaviour_ShouldReturnFailure_WhenMissingRequiredPermissions()
    {
        // Arrange
        List<string> roles = [SystemRoles.Administrator, SystemRoles.Employee, SystemRoles.Customer];
        List<string> permissions = [SystemPermissions.Users.Update];

        TestAdminRequest request = new();
        _currentUserProvider.IsAuthenticated.Returns(true);
        _currentUserProvider
            .GetCurrentUser()
            .Returns(new CurrentUser(UserId.From(Guid.NewGuid()), roles, permissions));

        // Act
        var result = await _sut.Handle(request, () => Task.FromResult(Result.Success()), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ResultType.Should().Be(ResultType.Unauthorized);
        result.Error?.HasErrors.Should().BeTrue();
        result.Error?.FirstError()?.Code.Should().Be("401");
        result.Error?.FirstError()?.Description.Should().Contain("User is Unauthorized from taking this action");
    }

    private record TestRequest : IAppBaseRequest;

    [Authorize(Roles = SystemRoles.Administrator, Permissions = SystemPermissions.Users.Delete)]
    private record TestAdminRequest : IAppBaseRequest;
}
