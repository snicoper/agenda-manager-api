using AgendaManager.Domain.Common.ValueObjects.EmailAddress;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.TestCommon.Constants;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class UserEmailTests
{
    private readonly User _user = UserFactory.CreateUser();

    private readonly IEmailUniquenessChecker _emailUniquenessChecker = Substitute.For<IEmailUniquenessChecker>();

    [Fact]
    public void UserEmail_ShouldRaiseEvent_WhenEmailIsConfirmed()
    {
        // Act
        _user.SetEmailConfirmed();

        // Assert
        _user.DomainEvents.Should().Contain(x => x is UserEmailConfirmedDomainEvent);
        _user.IsEmailConfirmed.Should().BeTrue();
    }

    [Fact]
    public async Task UserEmail_ShouldRaiseEvent_WhenUserEmailIsUpdated()
    {
        // Arrange
        var email = UserConstants.UserLexi.Email;
        _emailUniquenessChecker.IsUnique(Arg.Any<EmailAddress>(), Arg.Any<CancellationToken>()).Returns(true);

        // Act
        await _user.UpdateEmail(email, _emailUniquenessChecker, CancellationToken.None);

        // Assert
        _user.DomainEvents.Should().Contain(x => x is UserEmailUpdatedDomainEvent);
        _user.Email.Should().Be(email);
    }

    [Fact]
    public async Task UserEmail_ShouldNotRaiseEvent_WhenUserEmailIsNotUpdated()
    {
        // Arrange
        _emailUniquenessChecker.IsUnique(Arg.Any<EmailAddress>(), Arg.Any<CancellationToken>()).Returns(true);

        // Act
        await _user.UpdateEmail(_user.Email, _emailUniquenessChecker, CancellationToken.None);

        // Assert
        _user.DomainEvents.Should().NotContain(x => x is UserEmailUpdatedDomainEvent);
        _user.Email.Should().Be(_user.Email);
    }
}
