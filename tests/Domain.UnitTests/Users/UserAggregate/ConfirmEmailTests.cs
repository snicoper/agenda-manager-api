using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class ConfirmEmailTests
{
    [Fact]
    public void ConfirmEmail_ShouldRaiseUserEmailConfirmedEvent_WhenEmailIsConfirmed()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        user.ConfirmEmail();

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserEmailConfirmedDomainEvent);
        user.IsEmailConfirmed.Should().BeTrue();
    }

    [Fact]
    public void ConfirmEmail_ShouldNotRaiseUserEmailConfirmedEvent_WhenEmailIsAlreadyConfirmed()
    {
        // Arrange
        var user = UserFactory.CreateUser(emailConfirmed: true);

        // Act
        user.ConfirmEmail();

        // Assert
        user.DomainEvents.Should().NotContain(x => x is UserEmailConfirmedDomainEvent);
        user.IsEmailConfirmed.Should().BeTrue();
    }
}
