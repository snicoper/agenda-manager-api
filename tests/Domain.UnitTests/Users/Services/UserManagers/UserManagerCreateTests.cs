using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;
using AgendaManager.TestCommon.Constants;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Users.Services.UserManagers;

public class UserManagerCreateTests
{
    private readonly User _user;

    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IPasswordPolicy _passwordPolicy;
    private readonly UserManager _sut;

    public UserManagerCreateTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _passwordPolicy = Substitute.For<IPasswordPolicy>();
        _sut = new UserManager(_userRepository, _passwordHasher, _passwordPolicy);

        _user = UserFactory.CreateUserAlice();
    }

    [Fact]
    public async Task CreateUser_ShouldSuccess_WhenAllParametersAreValid()
    {
        // Act
        SetupPasswordPolicyValidatePassword(Result.Success());
        SetupPasswordHasherHashPassword();

        var userResult = await _sut.CreateUserAsync(
            userId: _user.Id,
            email: _user.Email,
            passwordRaw: UserConstants.UserAlice.RawPassword,
            firstName: UserConstants.UserAlice.FirstName,
            lastName: UserConstants.UserAlice.LastName);

        // Assert
        userResult.Should().BeOfType<Result<User>>();
        userResult.IsSuccess.Should().BeTrue();
        userResult.ResultType.Should().Be(ResultType.Created);
        userResult.Value!.Id.Should().Be(_user.Id);
        userResult.Value.Email.Should().Be(_user.Email);
        userResult.Value.IsActive.Should().BeTrue();
        userResult.Value.IsEmailConfirmed.Should().BeFalse();
        userResult.Value.DomainEvents.Should().Contain(x => x is UserCreatedDomainEvent);
    }

    [Fact]
    public async Task CreateUser_ShouldFailure_WhenEmailIsNotUnique()
    {
        // Arrange
        SetupPasswordPolicyValidatePassword(Result.Success());
        SetupPasswordHasherHashPassword();
        _userRepository.EmailExistsAsync(Arg.Any<EmailAddress>(), Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var userResult = await _sut.CreateUserAsync(
            userId: _user.Id,
            email: _user.Email,
            passwordRaw: UserConstants.UserAlice.RawPassword,
            firstName: UserConstants.UserAlice.FirstName,
            lastName: UserConstants.UserAlice.LastName);

        // Assert
        userResult.Should().BeOfType<Result<User>>();
        userResult.ResultType.Should().Be(ResultType.Validation);
        userResult.IsFailure.Should().BeTrue();
        userResult.Error?.FirstError()?.Description.Should().Be("The email already exists.");
    }

    [Fact]
    public async Task CreateUser_ShouldFailure_WhenPasswordHashingFails()
    {
        // Arrange
        SetupPasswordPolicyValidatePassword(UserErrors.InvalidFormatPassword);

        // Act
        var userResult = await _sut.CreateUserAsync(
            userId: _user.Id,
            email: _user.Email,
            passwordRaw: UserConstants.UserAlice.RawPassword,
            firstName: UserConstants.UserAlice.FirstName,
            lastName: UserConstants.UserAlice.LastName);

        // Assert
        userResult.Should().BeOfType<Result<User>>();
        userResult.ResultType.Should().Be(ResultType.Validation);
        userResult.Error?.FirstError()?.Description.Should()
            .Be(UserErrors.InvalidFormatPassword.FirstError()?.Description);
        userResult.IsFailure.Should().BeTrue();
    }

    private void SetupPasswordPolicyValidatePassword(Result resultExpected)
    {
        _passwordPolicy.ValidatePassword(Arg.Any<string>())
            .Returns(resultExpected);
    }

    private void SetupPasswordHasherHashPassword()
    {
        _passwordHasher.HashPassword(Arg.Any<string>())
            .Returns("hashedPassword");
    }
}
