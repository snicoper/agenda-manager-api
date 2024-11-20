using AgendaManager.Domain.Users.Enums;
using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class CreateUserTokenTests
{
    [Fact]
    public void CreateUserToken_ShouldCreateUserToken()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        var result = user.CreateUserToken(UserTokenType.EmailConfirmation, TimeSpan.FromHours(11));

        // Assert
        result.Should().NotBeNull();
        user.Tokens.Should().Contain(x => x == result.Value);
    }

    [Fact]
    public void CreateUserToken_ShouldRaiseEvent_WhenUserTokenIsCreated()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        var result = user.CreateUserToken(UserTokenType.EmailConfirmation, TimeSpan.FromHours(11));

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value?.DomainEvents.Should().Contain(x => x is UserTokenCreatedDomainEvent);
    }
}
