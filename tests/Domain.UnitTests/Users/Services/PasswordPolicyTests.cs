using AgendaManager.Domain.Users.Services;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.Services;

public class PasswordPolicyTests
{
    [Fact]
    public void PasswordPolicy_ValidatePasswordShouldReturnSuccess_WhenPasswordIsValid()
    {
        // Arrange
        const string rawPassword = "Password123!";
        var passwordPolicy = new PasswordPolicy();

        // Act
        var result = passwordPolicy.ValidatePassword(rawPassword);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Theory] // Require at least 8 characters, at least one uppercase letter, one lowercase letter, one number, and one special character.
    [InlineData("")] // Empty password.
    [InlineData("123456789")] // Password without uppercase letters.
    [InlineData("123456789!#$%&*()")] // Password without lowercase letters.
    [InlineData("password")] // Password without uppercase letters.
    [InlineData("Password")] // Password without numbers.
    [InlineData("Password123")] // Password without special characters.
    [InlineData("Password!")] // Password without special characters.
    [InlineData("Ps1!")] // Password with less than 8 characters.
    public void PasswordPolicy_ValidatePasswordShouldReturnFailure_WhenPasswordIsNotValid(string rawPassword)
    {
        // Arrange
        var passwordPolicy = new PasswordPolicy();

        // Act
        var result = passwordPolicy.ValidatePassword(rawPassword);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }
}
