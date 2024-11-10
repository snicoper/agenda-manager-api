using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class UserPasswordTests
{
    [Fact]
    public void UserPassword_ShouldRaiseEvent_WhenUserPasswordIsUpdate()
    {
        // Arrange
        var user = UserFactory.CreateUserBob();
        var passwordHashed = UserFactory
            .BcryptPasswordHasher
            .HashPassword(TestCommon.Seeds.Users.UserAlice.RawPassword);
        var passwordHash = PasswordHash.FromHashed(passwordHashed);

        // Act
        user.UpdatePassword(passwordHash);

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserPasswordUpdatedDomainEvent);
    }
}
