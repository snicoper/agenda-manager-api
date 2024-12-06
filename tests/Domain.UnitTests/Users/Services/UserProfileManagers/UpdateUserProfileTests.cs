using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Users.Services.UserProfileManagers;

public class UpdateUserProfileTests
{
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly UserProfileManager _sut;

    public UpdateUserProfileTests()
    {
        var userRepository = Substitute.For<IUserRepository>();
        _userProfileRepository = Substitute.For<IUserProfileRepository>();
        _sut = new UserProfileManager(_userProfileRepository, userRepository);
    }

    [Fact]
    public async Task Update_ShouldUpdate_WhenUserProfileIsUpdated()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        var result = await _sut.UpdateUserProfile(user, "John", "Doe", null, null, null, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Update_ShouldNotUpdate_WhenUserProfileIsNotUpdated()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        var result = await _sut.UpdateUserProfile(
            user,
            user.Profile.FirstName,
            user.Profile.LastName,
            null,
            null,
            null,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        user.DomainEvents.Should().NotContain(x => x is UserProfileUpdatedDomainEvent);
    }

    [Fact]
    public async Task Update_ShouldNotUpdate_WhenIdentityDocumentExists()
    {
        // Arrange
        var user = UserFactory.CreateUser();
        var userProfile = UserProfileFactory.CreateUserProfile();

        _userProfileRepository.IdentityDocumentExistsAsync(userProfile.IdentityDocument!, CancellationToken.None)
            .Returns(true);

        // Act
        var result = await _sut.UpdateUserProfile(
            user: user,
            firstName: "John",
            lastName: "Doe",
            phoneNumber: null,
            address: null,
            identityDocument: userProfile.IdentityDocument,
            cancellationToken: CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(UserProfileErrors.IdentityDocumentAlreadyExists.FirstError());
    }
}
