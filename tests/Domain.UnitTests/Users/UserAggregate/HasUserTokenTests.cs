using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class HasUserTokenTests
{
    [Fact]
    public void HasUserToken_ShouldReturnTrue_WhenUserTokenExists()
    {
        // Arrange
        var user = UserFactory.CreateUser();
        var userToken = UserTokenFactory.CreateUserToken();

        user.AddUserToken(userToken);

        // Act
        var result = user.HasUserToken(userToken.Id);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void HasUserToken_ShouldReturnFalse_WhenUserTokenDoesNotExist()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        var result = user.HasUserToken(UserTokenId.Create());

        // Assert
        result.Should().BeFalse();
    }
}
