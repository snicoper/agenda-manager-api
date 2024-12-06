using AgendaManager.Domain.Users.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.Entities.UserProfiles;

public class UserProfileCreateTests
{
    [Fact]
    public void Create_ShouldSuccess_WhenAllParametersAreValid()
    {
        // Act
        var userProfile = UserProfileFactory.CreateUserProfile();

        // Assert
        userProfile.Should().NotBeNull();
    }

    [Theory]
    [InlineData("Perico")]
    [InlineData("Perico de los Palotes")]
    [InlineData("Perico de los Palotes y sus amigos")]
    public void Create_ShouldThrowException_WhenFirstNameIsValid(string validFirstName)
    {
        // Act
        var action = () => UserProfileFactory.CreateUserProfile(firstName: validFirstName);

        // Assert
        action.Should().NotThrow<UserProfileDomainException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(101)]
    public void Create_ShouldThrowException_WhenFirstNameIsNotValid(int invalidFirstNameLength)
    {
        // Arrange
        var firstName = new string('*', invalidFirstNameLength);

        // Act
        var action = () => UserProfileFactory.CreateUserProfile(firstName: firstName);

        // Assert
        action.Should().Throw<UserProfileDomainException>();
    }

    [Theory]
    [InlineData("P3rico")]
    [InlineData("P$erico")]
    [InlineData("P3rico$")]
    [InlineData("Perico!")]
    public void Create_ShouldThrowException_WhenFirstNameContainsInvalidChar(string invalidFirstName)
    {
        // Act
        var action = () => UserProfileFactory.CreateUserProfile(firstName: invalidFirstName);

        // Assert
        action.Should().Throw<UserProfileDomainException>();
    }

    [Theory]
    [InlineData("Perico")]
    [InlineData("Perico de los Palotes")]
    [InlineData("Perico de los Palotes y sus amigos")]
    public void Create_ShouldThrowException_WhenLastNameIsValid(string validLastName)
    {
        // Act
        var action = () => UserProfileFactory.CreateUserProfile(lastName: validLastName);

        // Assert
        action.Should().NotThrow<UserProfileDomainException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(101)]
    public void Create_ShouldThrowException_WhenLastNameIsNotValid(int invalidLastNameLength)
    {
        // Arrange
        var lastName = new string('*', invalidLastNameLength);

        // Act
        var action = () => UserProfileFactory.CreateUserProfile(lastName: lastName);

        // Assert
        action.Should().Throw<UserProfileDomainException>();
    }

    [Theory]
    [InlineData("P3rico")]
    [InlineData("P$erico")]
    [InlineData("P3rico$")]
    [InlineData("Perico!")]
    public void Create_ShouldThrowException_WhenLastNameContainsInvalidChar(string invalidLastName)
    {
        // Act
        var action = () => UserProfileFactory.CreateUserProfile(lastName: invalidLastName);

        // Assert
        action.Should().Throw<UserProfileDomainException>();
    }
}
