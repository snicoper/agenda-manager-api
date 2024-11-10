using AgendaManager.Domain.Common.ValueObjects.EmailAddress;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Users.Services.UserManagers;

public class UserManagerUpdateTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly UserManager _sut;

    public UserManagerUpdateTests()
    {
        _sut = new UserManager(_userRepository);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldReturnSuccess_WhenUserIsUpdated()
    {
        // Arrange
        var user = UserFactory.CreateUser();
        const string newFirstName = "NewFirstName";
        const string newLastName = "NewLastName";
        _userRepository.EmailExistsAsync(Arg.Any<EmailAddress>(), Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await _sut.UpdateUserAsync(user, newFirstName, newLastName, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldReturnFailure_WhenUserIsUpdated()
    {
        // Arrange
        var user = UserFactory.CreateUser();
        const string newFirstName = "NewFirstName";
        const string newLastName = "NewLastName";
        _userRepository.EmailExistsAsync(Arg.Any<EmailAddress>(), Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _sut.UpdateUserAsync(user, newFirstName, newLastName, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be("Email already exists.");
    }
}
