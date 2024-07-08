using AgendaManager.Domain.Users.Exceptions;
using AgendaManager.Domain.Users.ValueObjects;
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
}
