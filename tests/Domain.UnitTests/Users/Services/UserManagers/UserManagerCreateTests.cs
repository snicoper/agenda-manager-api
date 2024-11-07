using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.EmailAddress;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.Exceptions;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Users.Services.UserManagers;

public class UserManagerCreateTests
{
    private readonly UserManager _sut;
    private readonly IUserRepository _userRepository;

    public UserManagerCreateTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _sut = new UserManager(_userRepository);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnResultSuccess_WhenUserIsCreated()
    {
        // Arrange
        var user = UserFactory.CreateUserAlice();

        // Act
        var userResult = await _sut.CreateAsync(
            userId: user.Id,
            email: user.Email,
            passwordHash: user.PasswordHash,
            firstName: user.FirstName,
            lastName: user.LastName);

        // Assert
        userResult.Should().BeOfType<Result<User>>();
        userResult.IsSuccess.Should().BeTrue();
        userResult.ResultType.Should().Be(ResultType.Created);
        userResult.Value!.Id.Should().Be(user.Id);
        userResult.Value.Email.Should().Be(user.Email);
        userResult.Value.FirstName.Should().Be(user.FirstName);
        userResult.Value.LastName.Should().Be(user.LastName);
        userResult.Value.Active.Should().BeTrue();
        userResult.Value.IsEmailConfirmed.Should().BeFalse();
        userResult.Value.DomainEvents.Should().Contain(x => x is UserCreatedDomainEvent);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnResultFailure_WhenEmailAlreadyExists()
    {
        var user = UserFactory.CreateUserAlice();

        // Act
        _userRepository.EmailExistsAsync(Arg.Any<User>(), Arg.Any<CancellationToken>()).Returns(true);
        var userResult = await _sut.CreateAsync(
            userId: user.Id,
            email: user.Email,
            passwordHash: user.PasswordHash,
            firstName: user.FirstName,
            lastName: user.LastName);

        // Assert
        userResult.Should().BeOfType<Result<User>>();
        userResult.ResultType.Should().Be(ResultType.Validation);
        userResult.IsFailure.Should().BeTrue();
        userResult.Error?.FirstError()?.Description.Should().Be("Email already exists.");
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowUserDomainException_WhenFirstNameIsGreaterThan256()
    {
        var firstName = new string('a', 257);

        // Act & Assert
        await Assert.ThrowsAsync<UserDomainException>(
            () => _sut.CreateAsync(
                userId: UserId.Create(),
                email: EmailAddress.From("email@example.com"),
                passwordHash: "passwordHash",
                firstName: firstName,
                lastName: "lastName"));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowUserDomainException_WhenLastNameIsGreaterThan256()
    {
        var lastName = new string('a', 257);

        // Act & Assert
        await Assert.ThrowsAsync<UserDomainException>(
            () => _sut.CreateAsync(
                userId: UserId.Create(),
                email: EmailAddress.From("email@example.com"),
                passwordHash: "passwordHash",
                firstName: "firstName",
                lastName: lastName));
    }
}
