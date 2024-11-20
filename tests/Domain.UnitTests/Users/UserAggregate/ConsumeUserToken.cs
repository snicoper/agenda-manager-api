using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class ConsumeUserToken
{
    [Fact]
    public void ConsumeUserToken_ShouldConsumeUserToken()
    {
        // Arrange
        var user = UserFactory.CreateUser();
        var userToken = UserTokenFactory.CreateUserToken();

        user.AddUserToken(userToken);

        // Act
        var result = user.ConsumeUserToken(userToken.Id, userToken.Token.Value);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void ConsumeUserToken_ShouldRaiseEvent_WhenUserTokenIsConsumed()
    {
        // Arrange
        var user = UserFactory.CreateUser();
        var userToken = UserTokenFactory.CreateUserToken();

        user.AddUserToken(userToken);

        // Act
        var result = user.ConsumeUserToken(userToken.Id, userToken.Token.Value);

        // Assert
        result.IsSuccess.Should().BeTrue();
        userToken.DomainEvents.Should().Contain(x => x is UserTokenConsumedDomainEvent);
    }

    [Fact]
    public void ConsumeUserToken_ShouldReturnFailure_WhenUserTokenIsNotFound()
    {
        // Arrange
        var user = UserFactory.CreateUser();
        var userTokenId = UserTokenId.Create();
        var userToken = UserTokenFactory.CreateUserToken();

        // Act
        var result = user.ConsumeUserToken(userTokenId, userToken.Token.Value);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(UserTokenErrors.UserTokenNotFound.FirstError());
    }
}
