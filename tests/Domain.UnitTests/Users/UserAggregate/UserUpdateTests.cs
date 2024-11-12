using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class UserUpdateTests
{
    [Fact]
    public void UserUpdate_ShouldRaiseEvent_WhenUserIsUpdated()
    {
        // Arrange
        var user = UserFactory.CreateUserAlice();
        const string firstName = "newFirstName";
        const string lastName = "newLastName";

        // Act
        user.Update(firstName, lastName);

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserUpdatedDomainEvent);
        user.FirstName.Should().Be(firstName);
        user.LastName.Should().Be(lastName);
    }
}
