using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.Entities.UserTokens;

public class UserTokenConsume
{
    private readonly UserToken _userToken = UserTokenFactory.CreateUserToken();

    [Fact]
    public void UserToken_ShouldReturnSuccess_WhenTokenInvalid()
    {
        // Arrange
        var token = _userToken.Token.Value;

        // Act
        var result = _userToken.Consume(token);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void UserToken_ShouldReturnFailureInvalidToken_WhenTokenIsInvalid()
    {
        // Arrange
        const string token = "Invalid token";

        // Act
        var result = _userToken.Consume(token);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be("User token is invalid.");
    }
}
