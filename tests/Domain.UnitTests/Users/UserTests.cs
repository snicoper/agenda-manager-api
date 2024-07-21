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
        var user = UserFactory.CreateUserAlice();

        // Assert
        user.Should().NotBeNull();
    }

    [Fact]
    public void User_ShouldRaiseEvent_WhenUserIsCreated()
    {
        // Act
        var user = UserFactory.CreateUserAlice();

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserCreatedDomainEvent);
    }

    [Fact]
    public void User_ShouldRaiseEvent_WhenUserIsUpdated()
    {
        // Arrange
        var user = UserFactory.CreateUserAlice();
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
    public void User_ShouldRaiseEvent_WhenUserPasswordIsChanged()
    {
        // Arrange
        var user = UserFactory.CreateUserBob();
        var passwordHash = UserFactory.BcryptPasswordHasher.HashPassword(Constants.UserAlice.RawPassword);

        // Act
        user.UpdatePassword(passwordHash);

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserPasswordUpdatedDomainEvent);
    }

    [Fact]
    public void User_ShouldRaiseEvent_WhenUpdateRefreshTokenIsCalled()
    {
        // Arrange
        var user = UserFactory.CreateUserCarol();
        var refreshToken = RefreshToken.Generate(TimeSpan.FromDays(1));

        // Act
        user.UpdateRefreshToken(refreshToken);

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserRefreshTokenUpdatedDomainEvent);
    }

    [Fact]
    public void User_ShouldAddRefreshToken_WhenIsSet()
    {
        // Arrange
        var user = UserFactory.CreateUserAlice();
        var token = Guid.NewGuid().ToString();
        var expiryTime = DateTimeOffset.UtcNow.AddDays(1);
        var newRefreshToken = RefreshToken.Create(token, expiryTime);

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
        var user = UserFactory.CreateUserAlice();

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
        var user = UserFactory.CreateUserAlice();

        // Assert
        user.Active.Should().BeTrue();
    }

    [Fact]
    public void User_ShouldRaiseEvent_WhenUserIsActiveStateIsChanged()
    {
        // Arrange
        var user = UserFactory.CreateUserAlice();

        // Act
        user.SetActiveState(false);

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserActiveStateChangedDomainEvent);
        user.Active.Should().BeFalse();
    }

    [Fact]
    public void User_ShouldRaiseEvent_WhenAddingRole()
    {
        // Arrange
        var user = UserFactory.CreateUserAlice();
        var role = RoleFactory.CreateRoleAdmin();

        // Act
        user.AddRole(role);

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserRoleAddedDomainEvent);
        user.Roles.Should().Contain(role);
        user.Roles.Should().HaveCount(1);
        user.Roles.Should().ContainSingle(x => x.Id == role.Id);
    }

    [Fact]
    public void User_ShouldRaiseEvent_WhenRemovingRole()
    {
        // Arrange
        var user = UserFactory.CreateUserAlice();
        var role = RoleFactory.CreateRoleAdmin();
        user.AddRole(role);

        // Act
        user.RemoveRole(role);

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserRoleRemovedDomainEvent);
        user.Roles.Should().NotContain(role);
        user.Roles.Should().HaveCount(0);
    }

    [Fact]
    public void User_ShouldRaiseEvent_WhenUserEmailIsUpdated()
    {
        // Arrange
        var user = UserFactory.CreateUserCarol();
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
        var user = UserFactory.CreateUserLexi();

        // Act
        user.UpdateEmail(user.Email);

        // Assert
        user.DomainEvents.Should().NotContain(x => x is UserEmailUpdatedDomainEvent);
        user.Email.Should().Be(user.Email);
    }
}
