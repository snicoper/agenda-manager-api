using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Exceptions;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Users.ValueObjects;

public class PasswordHashTests
{
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
    private readonly IPasswordPolicy _passwordPolicy = Substitute.For<IPasswordPolicy>();

    [Fact]
    public void PasswordHash_ShouldReturnCorrectHash_WhenValidPasswordIsProvidedInFromHashed()
    {
        // Arrange
        const string hashedPassword = "validPassword";

        // Act
        var passwordHash = PasswordHash.FromHashed(hashedPassword);

        // Assert
        passwordHash.HashedValue.Should().NotBeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void PasswordHash_ShouldReturnThrowException_WhenInvalidPasswordIsProvidedInFromHashed(string hashedPassword)
    {
        // Act
        var passwordHash = () => PasswordHash.FromHashed(hashedPassword);

        // Assert
        passwordHash.Should().Throw<UserDomainException>();
        passwordHash.Should().Throw<UserDomainException>().WithMessage("Hashed password cannot be empty.");
    }

    [Fact]
    public void PasswordHash_ShouldReturnResultSuccess_WhenValidPasswordIsProvidedInFromRaw()
    {
        // Arrange
        const string rawPassword = "validPassword";
        _passwordPolicy.ValidatePassword(Arg.Any<string>()).Returns(Result.Success());
        _passwordHasher.HashPassword(Arg.Any<string>()).Returns(rawPassword);

        // Act
        var passwordHashResult = PasswordHash.FromRaw(rawPassword, _passwordHasher, _passwordPolicy);

        // Assert
        passwordHashResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void PasswordHash_ShouldReturnResultFailure_WhenInValidPasswordIsProvidedInFromRaw()
    {
        // Arrange
        const string rawPassword = "validPassword";
        _passwordPolicy.ValidatePassword(Arg.Any<string>()).Returns(Result.Failure());

        // Act
        var passwordHashResult = PasswordHash.FromRaw(rawPassword, _passwordHasher, _passwordPolicy);

        // Assert
        passwordHashResult.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void PasswordHash_ShouldReturnTrue_WhenVerifyIsSuccessful()
    {
        // Arrange
        const string rawPassword = "validPassword";
        _passwordPolicy.ValidatePassword(Arg.Any<string>()).Returns(Result.Success());
        _passwordHasher.VerifyPassword(Arg.Any<string>(), Arg.Any<PasswordHash>()).Returns(true);

        // Act
        var passwordHash = PasswordHash.FromRaw(rawPassword, _passwordHasher, _passwordPolicy);
        var verifyResult = passwordHash.Value?.Verify(rawPassword, _passwordHasher);

        // Assert
        verifyResult.Should().BeTrue();
    }

    [Fact]
    public void PasswordHash_ShouldReturnFalse_WhenVerifyIsUnSuccessful()
    {
        // Arrange
        const string rawPassword = "validPassword";
        const string rawPasswordVerify = "validPassword2";
        _passwordPolicy.ValidatePassword(Arg.Any<string>()).Returns(Result.Success());
        _passwordHasher.VerifyPassword(Arg.Any<string>(), Arg.Any<PasswordHash>()).Returns(false);

        // Act
        var passwordHash = PasswordHash.FromRaw(rawPassword, _passwordHasher, _passwordPolicy);
        var verifyResult = passwordHash.Value?.Verify(rawPasswordVerify, _passwordHasher);

        // Assert
        verifyResult.Should().BeFalse();
    }
}
