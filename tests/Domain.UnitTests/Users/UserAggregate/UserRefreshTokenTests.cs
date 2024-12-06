using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class UserRefreshTokenTests
{
    private readonly User _user = UserFactory.CreateUser();

    [Fact]
    public void UserRefreshToken_ShouldRaiseEvent_WhenUpdateRefreshTokenIsUpdate()
    {
        // Arrange
        var refreshToken = Token.Generate(TimeSpan.FromDays(1));

        // Act
        _user.UpdateRefreshToken(refreshToken);

        // Assert
        _user.DomainEvents.Should().Contain(x => x is UserRefreshTokenUpdatedDomainEvent);
    }

    [Fact]
    public void UserRefreshToken_ShouldAddRefreshToken_WhenIsUpdate()
    {
        // Arrange
        var token = Guid.NewGuid().ToString();
        var expires = DateTimeOffset.UtcNow.AddDays(1);
        var newRefreshToken = Token.From(token, expires);

        // Act
        _user.UpdateRefreshToken(newRefreshToken);

        // Assert
        _user.RefreshToken.Should().NotBeNull();
        _user.RefreshToken.Should().Be(newRefreshToken);
    }
}
