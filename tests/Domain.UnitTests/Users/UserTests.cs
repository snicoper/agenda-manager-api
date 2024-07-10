using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.TestCommon.Factories;
using AgendaManager.TestCommon.TestConstants;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users;

public class UserTests
{
    [Fact]
    public void User_ShouldReturnUser_WhenUserIsCreated()
    {
        // Act
        var user = UserFactory.CreateUser();

        // Assert
        user.Should().NotBeNull();
    }

    [Fact]
    public void User_ShouldRaiseEvent_WhenUserIsCreated()
    {
        // Act
        var user = UserFactory.CreateUser();

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserCreatedDomainEvent);
    }

    [Fact]
    public void User_ShouldRaiseEvent_WhenUserIsUpdated()
    {
        // Arrange
        var user = UserFactory.CreateUser();
        const string firstName = "newFirstName";
        const string lastName = "newLastName";

        // Act
        user.UpdateUser(firstName, lastName);

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserUpdatedDomainEvent);
        user.FirstName.Should().Be(firstName);
        user.LastName.Should().Be(lastName);
    }

    [Fact]
    public void User_ShouldRaiseEvent_WhenUserPasswordIsUpdated()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        user.UpdatePassword("newPassword!34");

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserPasswordUpdatedDomainEvent);
    }

    [Fact]
    public void User_ShouldReturnTrue_WhenUserPasswordIsValid()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        var result = user.VerifyPassword(Constants.Users.Password);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void User_ShouldReturnFalse_WhenUserPasswordIsInvalid()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        var result = user.VerifyPassword("wrongPassword123@@");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void User_ShouldNotRaiseEvent_WhenUserPasswordIsSame()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        user.UpdatePassword(Constants.Users.Password);

        // Assert
        user.DomainEvents.Should().NotContain(x => x is UserPasswordUpdatedDomainEvent);
    }

    [Fact]
    public void User_ShouldRaiseEvent_WhenUserEmailIsUpdated()
    {
        // Arrange
        var user = UserFactory.CreateUser();
        var email = EmailAddress.From("new@example.com");

        // Act
        user.UpdateEmail(email);

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserEmailUpdatedDomainEvent);
        user.Email.Should().Be(email);
    }

    [Fact]
    public void User_ShouldNotRaiseEvent_WhenUserEmailIsSame()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        user.UpdateEmail(user.Email);

        // Assert
        user.DomainEvents.Should().NotContain(x => x is UserEmailUpdatedDomainEvent);
        user.Email.Should().Be(user.Email);
    }

    [Fact]
    public void User_ShouldAddRefreshToken_WhenIsSet()
    {
        // Arrange
        var user = UserFactory.CreateUser();
        var newRefreshToken = RefreshToken.Create("token", DateTimeOffset.Now);

        // Act
        user.UpdateRefreshToken(newRefreshToken);

        // Assert
        user.RefreshToken.Should().NotBeNull();
        user.RefreshToken.Should().Be(newRefreshToken);
    }

    [Fact]
    public void User_ShouldRaiseEvent_WhenConfirmEmail()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        user.ConfirmEmail();

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserEmailConfirmedDomainEvent);
        user.IsEmailConfirmed.Should().BeTrue();
    }

    [Fact]
    public void User_ShouldActiveTrue_WhenUserIsACreated()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Assert
        user.Active.Should().BeTrue();
    }

    [Fact]
    public void User_ShouldRaiseEvent_WhenUserIsActiveStateIsChanged()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        user.SetActiveState(false);

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserActiveStateChangedDomainEvent);
        user.Active.Should().BeFalse();
    }
}
