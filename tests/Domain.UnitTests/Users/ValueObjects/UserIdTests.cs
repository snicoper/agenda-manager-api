using AgendaManager.Domain.Users.ValueObjects;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.ValueObjects;

public class UserIdTests
{
    [Fact]
    public void UserId_ShouldCreateNewUserId_WhenValidGuidIsPassed()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var userId = UserId.From(guid);

        // Assert
        userId.Value.Should().Be(guid);
    }

    [Fact]
    public void UserId_ShouldCreateNewUserId_WhenCreateWithOutGuid()
    {
        // Act
        var userId = UserId.Create();

        // Assert
        userId.Value.Should().NotBeEmpty();
    }
}
