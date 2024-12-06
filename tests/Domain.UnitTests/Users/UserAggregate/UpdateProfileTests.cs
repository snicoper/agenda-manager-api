using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class UpdateProfileTests
{
    [Fact]
    public void UpdateProfile_ShouldUpdate_WhenUserProfileIsValidProvided()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        user.UpdateProfile("John", "Doe");

        // Assert
        user.Profile.FirstName.Should().Be("John");
    }

    [Fact]
    public void UpdateProfile_ShouldRaiseEvent_WhenUserProfileIsValidProvided()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        user.UpdateProfile("John", "Doe");

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserProfileUpdatedDomainEvent);
    }

    [Fact]
    public void UpdateProfile_ShouldNotRaiseEvent_WhenUserProfileIsSame()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        user.UpdateProfile(user.Profile.FirstName, user.Profile.LastName);

        // Assert
        user.DomainEvents.Should().NotContain(x => x is UserProfileUpdatedDomainEvent);
    }
}
