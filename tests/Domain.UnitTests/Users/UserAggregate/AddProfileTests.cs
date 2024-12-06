using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class AddProfileTests
{
    [Fact]
    public void AddProfile_ShouldCreate_WhenUserIsProvided()
    {
        // Arrange
        var user = UserFactory.CreateUser();
        var profile = UserProfileFactory.CreateUserProfile();

        // Act
        user.AddProfile(profile);

        // Assert
        user.Profile.Should().BeEquivalentTo(profile);
    }

    [Fact]
    public void AddProfile_ShouldRaiseEvent_WhenUserIsProvided()
    {
        // Arrange
        var user = UserFactory.CreateUser();
        var profile = UserProfileFactory.CreateUserProfile();

        // Act
        user.AddProfile(profile);

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserProfileAddedDomainEvent);
    }
}
