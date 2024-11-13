using AgendaManager.Domain.Users.Services;
using AgendaManager.TestCommon.Constants;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.Services;

public class PasswordPolicyTests
{
    private readonly PasswordPolicy _passwordPolicy = new();

    [Fact]
    public void PasswordPolicy_ValidatePasswordShouldReturnSuccess_WhenPasswordIsValid()
    {
        // Act
        var result = _passwordPolicy.ValidatePassword(UserConstants.UserBob.RawPassword);

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
        // Act
        var result = _passwordPolicy.ValidatePassword(rawPassword);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }
}
