using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.Entities.UserProfiles;

public class UserProfileHasChangesTests
{
    [Fact]
    public void UserHasChanges_ShouldReturnTrue_WhenUserHasChanges()
    {
        // Arrange
        var userProfile = UserProfileFactory.CreateUserProfile();

        // Act
        var hasChanges = userProfile.HasChanges(
            "New FirstName",
            userProfile.LastName,
            userProfile.PhoneNumber,
            userProfile.Address,
            userProfile.IdentityDocument);

        // Assert
        hasChanges.Should().BeTrue();
    }

    [Fact]
    public void UserHasChanges_ShouldReturnFalse_WhenUserHasNoChanges()
    {
        // Arrange
        var userProfile = UserProfileFactory.CreateUserProfile();

        // Act
        var hasChanges = userProfile.HasChanges(
            userProfile.FirstName,
            userProfile.LastName,
            userProfile.PhoneNumber,
            userProfile.Address,
            userProfile.IdentityDocument);

        // Assert
        hasChanges.Should().BeFalse();
    }
}
