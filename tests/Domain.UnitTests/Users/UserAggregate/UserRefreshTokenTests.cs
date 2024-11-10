using AgendaManager.Domain.Common.ValueObjects.Token;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class UserRefreshTokenTests
{
    [Fact]
    public void UserRefreshToken_ShouldRaiseEvent_WhenUpdateRefreshTokenIsUpdate()
    {
        // Arrange
        var user = UserFactory.CreateUserCarol();
        var refreshToken = Token.Generate(TimeSpan.FromDays(1));

        // Act
        user.UpdateRefreshToken(refreshToken);

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserRefreshTokenUpdatedDomainEvent);
    }

    [Fact]
    public void UserRefreshToken_ShouldAddRefreshToken_WhenIsUpdate()
    {
        // Arrange
        var user = UserFactory.CreateUserAlice();
        var token = Guid.NewGuid().ToString();
        var expires = DateTimeOffset.UtcNow.AddDays(1);
        var newRefreshToken = Token.From(token, expires);

        // Act
        user.UpdateRefreshToken(newRefreshToken);

        // Assert
        user.RefreshToken.Should().NotBeNull();
        user.RefreshToken.Should().Be(newRefreshToken);
    }
}
