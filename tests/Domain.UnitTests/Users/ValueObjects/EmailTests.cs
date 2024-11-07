using AgendaManager.Domain.Common.ValueObjects.EmailAddress;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.ValueObjects;

public class EmailTests
{
    [Fact]
    public void Email_ShouldCreateNewEmail_WhenValidEmailIsPassed()
    {
        // Arrange
        const string email = "test@test.com";

        // Act
        var emailValueObject = EmailAddress.From(email);

        // Assert
        emailValueObject.Value.Should().Be(email);
    }

    [Theory]
    [InlineData("test@test")]
    [InlineData("test@test.c")]
    [InlineData("test")]
    [InlineData("test@")]
    [InlineData("test@123123")]
    public void Email_ShouldThrowInvalidEmailException_WhenInvalidEmailIsPassed(string email)
    {
        // Act
        var act = () => EmailAddress.From(email);

        // Assert
        act.Should().Throw<InvalidEmailAddressException>();
    }

    [Fact]
    public void Email_ShouldThrowInvalidEmailException_WhenEmailLengthIsGreaterThan256()
    {
        // Arrange
        var emailName = new string('a', 248);
        var email = $"{emailName}@test.com";

        // Act
        var act = () => EmailAddress.From(email);

        // Assert
        act.Should().Throw<InvalidEmailAddressException>();
    }
}
