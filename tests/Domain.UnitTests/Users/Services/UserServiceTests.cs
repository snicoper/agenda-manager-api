using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Users.Services;

public class UserServiceTests
{
    private readonly UserService _sut;
    private readonly IUserRepository _userRepository;

    public UserServiceTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _sut = new UserService(_userRepository);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnResultSuccess_WhenUserIsCreated()
    {
        // Arrange
        var user = UserFactory.CreateUserAlice();

        // Act
        var result = await _sut.CreateAsync(
            userId: user.Id,
            email: user.Email,
            passwordHash: user.PasswordHash,
            firstName: user.FirstName,
            lastName: user.LastName,
            active: false,
            confirmEmail: true,
            cancellationToken: CancellationToken.None);

        // Assert
        result.Should().BeOfType<Result<User>>();
        result.IsSuccess.Should().BeTrue();
        result.ResultType.Should().Be(ResultType.Created);
        result.Value!.Id.Should().Be(user.Id);
        result.Value.Email.Should().Be(user.Email);
        result.Value.FirstName.Should().Be(user.FirstName);
        result.Value.LastName.Should().Be(user.LastName);
        result.Value.Active.Should().BeFalse();
        result.Value.IsEmailConfirmed.Should().BeTrue();
        result.Value.DomainEvents.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnResultFailure_WhenEmailAlreadyExists()
    {
        var user = UserFactory.CreateUserAlice();

        // Act
        _userRepository.EmailExistsAsync(Arg.Any<User>(), Arg.Any<CancellationToken>()).Returns(true);
        var result = await _sut.CreateAsync(
            userId: user.Id,
            email: user.Email,
            passwordHash: user.PasswordHash,
            firstName: user.FirstName,
            lastName: user.LastName);

        // Assert
        result.Should().BeOfType<Result<User>>();
        result.ResultType.Should().Be(ResultType.Validation);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<Error>();
        result.Error?.FirstError()?.Description.Should().Be("Email already exists.");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnResultFailure_WhenFirstNameIsGreaterThan256()
    {
        var user = UserFactory.CreateUserAlice();
        var firstName = new string('a', 257);

        // Act
        var result = await _sut.CreateAsync(
            userId: user.Id,
            email: user.Email,
            passwordHash: user.PasswordHash,
            firstName: firstName,
            lastName: user.LastName);

        // Assert
        result.Should().BeOfType<Result<User>>();
        result.ResultType.Should().Be(ResultType.Conflict);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<Error>();
        result.Error?.FirstError()?.Description.Should().Be("First name exceeds length.");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnResultFailure_WhenLastNameIsGreaterThan256()
    {
        var user = UserFactory.CreateUserAlice();
        var lastName = new string('a', 257);

        // Act
        var result = await _sut.CreateAsync(
            userId: user.Id,
            email: user.Email,
            passwordHash: user.PasswordHash,
            firstName: user.FirstName,
            lastName: lastName);

        // Assert
        result.Should().BeOfType<Result<User>>();
        result.ResultType.Should().Be(ResultType.Conflict);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<Error>();
        result.Error?.FirstError()?.Description.Should().Be("Last name exceeds length.");
    }
}
