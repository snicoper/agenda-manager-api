using AgendaManager.Domain.Common.Exceptions;
using AgendaManager.Domain.Common.ValueObjects;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.ValueObjects.EmailAddressTests;

public class EmailAddressTests
{
    [Fact]
    public void EmailAddress_ShouldCreateNewEmail_WhenValidEmailIsProvided()
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
    public void EmailAddress_ShouldThrowInvalidEmailException_WhenInvalidEmailIsProvided(string email)
    {
        // Act
        var action = () => EmailAddress.From(email);

        // Assert
        action.Should().Throw<DomainException>();
    }

    [Fact]
    public void EmailAddress_ShouldThrowInvalidEmailException_WhenEmailLengthIsGreaterThan256()
    {
        // Arrange
        var emailName = new string('a', 257);
        var email = $"{emailName}@test.com";

        // Act
        var action = () => EmailAddress.From(email);

        // Assert
        action.Should().Throw<DomainException>();
    }
}
