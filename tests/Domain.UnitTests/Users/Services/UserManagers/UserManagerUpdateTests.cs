using AgendaManager.Domain.Common.ValueObjects.EmailAddress;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Users.Services.UserManagers;

public class UserManagerUpdateTests
{
    private const string _newFirstName = "NewFirstName";
    private const string _newLastName = "NewLastName";
    private readonly User _user;

    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly UserManager _sut;

    public UserManagerUpdateTests()
    {
        _sut = new UserManager(_userRepository);

        _user = UserFactory.CreateUser();
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldReturnSuccess_WhenUserIsUpdated()
    {
        // Arrange
        _userRepository.EmailExistsAsync(Arg.Any<EmailAddress>(), Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await _sut.UpdateUserAsync(_user, _newFirstName, _newLastName, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldReturnFailure_WhenUserIsUpdated()
    {
        // Arrange
        _userRepository.EmailExistsAsync(Arg.Any<EmailAddress>(), Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _sut.UpdateUserAsync(_user, _newFirstName, _newLastName, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be("Email already exists.");
    }
}
