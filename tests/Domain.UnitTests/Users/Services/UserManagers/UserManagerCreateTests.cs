using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Users;
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
    private readonly UserManager _sut;

    public UserManagerCreateTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _sut = new UserManager(_userRepository);

        _user = UserFactory.CreateUserAlice();
    }

    [Fact]
    public async Task CreateUserAsync_ShouldReturnResultSuccess_WhenUserIsCreated()
    {
        // Act
        var userResult = await _sut.CreateUserAsync(
            userId: _user.Id,
            email: _user.Email,
            passwordHash: _user.PasswordHash,
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
    public async Task CreateUserAsync_ShouldReturnResultFailure_WhenEmailAlreadyExists()
    {
        // Arrange
        _userRepository.EmailExistsAsync(Arg.Any<EmailAddress>(), Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var userResult = await _sut.CreateUserAsync(
            userId: _user.Id,
            email: _user.Email,
            passwordHash: _user.PasswordHash,
            firstName: UserConstants.UserAlice.FirstName,
            lastName: UserConstants.UserAlice.LastName);

        // Assert
        userResult.Should().BeOfType<Result<User>>();
        userResult.ResultType.Should().Be(ResultType.Validation);
        userResult.IsFailure.Should().BeTrue();
        userResult.Error?.FirstError()?.Description.Should().Be("The email already exists.");
    }
}
