using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Behaviours;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Application.Common.Models.Users;
using AgendaManager.Domain.Common.Constants;
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
    public async Task Handle_ShouldReturnResultSuccess_WhenNotRequiredRolesOrPermissions()
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
    public async Task Handle_ShouldReturnResultSuccess_WhenUserIsAuthenticated()
    {
        // Arrange
        List<string> roles = [Roles.Admin, Roles.Manager, Roles.Client];
        List<string> permissions = [Permissions.User.Delete];

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
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserIsNotAuthenticated()
    {
        // Arrange;
        TestAdminRequest request = new();
        _currentUserProvider.IsAuthenticated.Returns(false);

        // Act
        var result = async () => await _sut
            .Handle(request, () => Task.FromResult(Result.Success()), CancellationToken.None);

        // Assert
        await result.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenCurrentUserIsNull()
    {
        // Arrange;
        TestAdminRequest request = new();
        var currentUser = null as CurrentUser;
        _currentUserProvider.IsAuthenticated.Returns(true);
        _currentUserProvider.GetCurrentUser().Returns(currentUser);

        // Act
        var result = async () => await _sut
            .Handle(request, () => Task.FromResult(Result.Success()), CancellationToken.None);

        // Assert
        await result.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task Handle_ShouldReturnResultFailure_WhenUserIsNotHaveRequiredRoles()
    {
        // Arrange
        List<string> roles = [Roles.Manager, Roles.Client];
        List<string> permissions = [Permissions.User.Delete];

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
        result.Error?.FirstError()?.Code.Should().Be("Unauthorized");
        result.Error?.FirstError()?.Description.Should().Contain("User is Unauthorized from taking this action");
    }

    [Fact]
    public async Task Handle_ShouldReturnResultFailure_WhenUserIsNotHaveRequiredPermissions()
    {
        // Arrange
        List<string> roles = [Roles.Admin, Roles.Manager, Roles.Client];
        List<string> permissions = [Permissions.User.Update];

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
        result.Error?.FirstError()?.Code.Should().Be("Unauthorized");
        result.Error?.FirstError()?.Description.Should().Contain("User is Unauthorized from taking this action");
    }

    private record TestRequest : IAppBaseRequest;

    [Authorize(Roles = Roles.Admin, Permissions = Permissions.User.Delete)]
    private record TestAdminRequest : IAppBaseRequest;
}
