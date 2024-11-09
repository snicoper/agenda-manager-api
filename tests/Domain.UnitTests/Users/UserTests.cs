﻿using AgendaManager.Domain.Common.ValueObjects.EmailAddress;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Users;

public class UserTests
{
    private readonly IEmailUniquenessChecker _emailUniquenessChecker = Substitute.For<IEmailUniquenessChecker>();

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
    public void User_ShouldRaiseEvent_WhenUserPasswordIsUpdate()
    {
        // Arrange
        var user = UserFactory.CreateUserBob();
        var passwordHashed = UserFactory
            .BcryptPasswordHasher
            .HashPassword(TestCommon.Seeds.Users.UserAlice.RawPassword);
        var passwordHash = PasswordHash.FromHashed(passwordHashed);

        // Act
        user.UpdatePassword(passwordHash);

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserPasswordUpdatedDomainEvent);
    }

    [Fact]
    public void User_ShouldRaiseEvent_WhenUpdateRefreshTokenIsUpdate()
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
    public void User_ShouldAddRefreshToken_WhenIsUpdate()
    {
        // Arrange
        var user = UserFactory.CreateUserAlice();
        var token = Guid.NewGuid().ToString();
        var expires = DateTimeOffset.UtcNow.AddDays(1);
        var newRefreshToken = RefreshToken.From(token, expires);

        // Act
        user.UpdateRefreshToken(newRefreshToken);

        // Assert
        user.RefreshToken.Should().NotBeNull();
        user.RefreshToken.Should().Be(newRefreshToken);
    }

    [Fact]
    public void User_ShouldRaiseEvent_WhenEmailIsConfirmed()
    {
        // Arrange
        var user = UserFactory.CreateUserAlice();

        // Act
        user.SetEmailConfirmed();

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
    public void User_ShouldRaiseEvent_WhenUserIsActiveStateIsUpdated()
    {
        // Arrange
        var user = UserFactory.CreateUserAlice();

        // Act
        user.UpdateActiveState(false);

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserActiveStateUpdatetedDomainEvent);
        user.Active.Should().BeFalse();
    }

    [Fact]
    public void User_ShouldRaiseEvent_WhenRoleIsAdded()
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
    public void User_ShouldRaiseEvent_WhenRoleIsRemoved()
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
    public async Task User_ShouldRaiseEvent_WhenUserEmailIsUpdated()
    {
        // Arrange
        var user = UserFactory.CreateUserCarol();
        var email = EmailAddress.From("new@example.com");

        _emailUniquenessChecker.IsUnique(Arg.Any<EmailAddress>(), Arg.Any<CancellationToken>()).Returns(true);

        // Act
        await user.UpdateEmail(email, _emailUniquenessChecker, CancellationToken.None);

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserEmailUpdatedDomainEvent);
        user.Email.Should().Be(email);
    }

    [Fact]
    public async Task User_ShouldNotRaiseEvent_WhenUserEmailIsNotUpdated()
    {
        // Arrange
        var user = UserFactory.CreateUserLexi();

        _emailUniquenessChecker.IsUnique(Arg.Any<EmailAddress>(), Arg.Any<CancellationToken>()).Returns(true);

        // Act
        await user.UpdateEmail(user.Email, _emailUniquenessChecker, CancellationToken.None);

        // Assert
        user.DomainEvents.Should().NotContain(x => x is UserEmailUpdatedDomainEvent);
        user.Email.Should().Be(user.Email);
    }
}
