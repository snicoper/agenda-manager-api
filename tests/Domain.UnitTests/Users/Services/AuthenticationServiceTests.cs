using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Users.Services;

public class AuthenticationServiceTests
{
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
    private readonly AuthenticationService _sut;

    public AuthenticationServiceTests()
    {
        _sut = new AuthenticationService(_passwordHasher);
    }

    [Fact]
    public void Authenticate_ShouldReturnResultSuccess_WhenCredentialsAreValid()
    {
        // Arrange
        var user = UserFactory.CreateUser(emailConfirmed: true);
        _passwordHasher.VerifyPassword(Arg.Any<string>(), Arg.Any<PasswordHash>()).Returns(true);

        // Act
        var result = _sut.AuthenticateUser(user, "password");

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Authenticate_ShouldReturnResultFailure_WhenPasswordsDoNotMatch()
    {
        // Arrange
        var user = UserFactory.CreateUser(emailConfirmed: true);
        _passwordHasher.VerifyPassword(Arg.Any<string>(), Arg.Any<PasswordHash>()).Returns(false);

        // Act
        var result = _sut.AuthenticateUser(user, "password");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error?.FirstError()?.Description.Should().Be("Invalid credentials.");
    }

    [Fact]
    public void Authenticate_ShouldReturnResultFailure_WhenUserEmailConfirmedIsFalse()
    {
        // Arrange
        var user = UserFactory.CreateUser(emailConfirmed: false);
        _passwordHasher.VerifyPassword(Arg.Any<string>(), Arg.Any<PasswordHash>()).Returns(true);

        // Act
        var result = _sut.AuthenticateUser(user, "password");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error?.FirstError()?.Description.Should().Be("Email is not confirmed.");
    }

    [Fact]
    public void Authenticate_ShouldReturnResultFailure_WhenUserActiveIsFalse()
    {
        // Arrange
        var user = UserFactory.CreateUser(emailConfirmed: true, isActive: false);
        _passwordHasher.VerifyPassword(Arg.Any<string>(), Arg.Any<PasswordHash>()).Returns(true);

        // Act
        var result = _sut.AuthenticateUser(user, "password");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error?.FirstError()?.Description.Should().Be("User is not active.");
    }
}
