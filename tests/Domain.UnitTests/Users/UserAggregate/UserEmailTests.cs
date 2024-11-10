using AgendaManager.Domain.Common.ValueObjects.EmailAddress;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Users.Aggregates.UserAggregate;

public class UserEmailTests
{
    private readonly IEmailUniquenessChecker _emailUniquenessChecker = Substitute.For<IEmailUniquenessChecker>();

    [Fact]
    public void UserEmail_ShouldRaiseEvent_WhenEmailIsConfirmed()
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
    public async Task UserEmail_ShouldRaiseEvent_WhenUserEmailIsUpdated()
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
    public async Task UserEmail_ShouldNotRaiseEvent_WhenUserEmailIsNotUpdated()
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
