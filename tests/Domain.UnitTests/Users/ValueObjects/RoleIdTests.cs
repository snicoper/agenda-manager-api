using AgendaManager.Domain.Users.ValueObjects;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.ValueObjects;

public class RoleIdTests
{
    [Fact]
    public void PermissionId_ShouldCreateNewUserId_WhenValidGuidIsPassed()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var userId = RoleId.From(guid);

        // Assert
        userId.Value.Should().Be(guid);
    }

    [Fact]
    public void PermissionId_ShouldCreateNewUserId_WhenCreateWithOutGuid()
    {
        // Act
        var userId = RoleId.Create();

        // Assert
        userId.Value.Should().NotBeEmpty();
    }
}
