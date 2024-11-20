using AgendaManager.Domain.Common.ValueObjects.EmailAddress;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.TestCommon.Constants;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class UpdateEmailTests
{
    private readonly User _user = UserFactory.CreateUser();

    private readonly IEmailUniquenessPolicy _emailUniquenessPolicy = Substitute.For<IEmailUniquenessPolicy>();

    [Fact]
    public async Task UpdateEmail_ShouldRaiseUserEmailUpdatedEvent_WhenEmailIsUpdated()
    {
        // Arrange
        var email = UserConstants.UserLexi.Email;
        _emailUniquenessPolicy.IsUnique(Arg.Any<EmailAddress>(), Arg.Any<CancellationToken>()).Returns(true);

        // Act
        await _user.UpdateEmail(email, _emailUniquenessPolicy, CancellationToken.None);

        // Assert
        _user.DomainEvents.Should().Contain(x => x is UserEmailUpdatedDomainEvent);
        _user.Email.Should().Be(email);
    }

    [Fact]
    public async Task UpdateEmail_ShouldNotRaiseUserEmailUpdatedEvent_WhenEmailIsNotUpdated()
    {
        // Arrange
        _emailUniquenessPolicy.IsUnique(Arg.Any<EmailAddress>(), Arg.Any<CancellationToken>()).Returns(true);

        // Act
        await _user.UpdateEmail(_user.Email, _emailUniquenessPolicy, CancellationToken.None);

        // Assert
        _user.DomainEvents.Should().NotContain(x => x is UserEmailUpdatedDomainEvent);
        _user.Email.Should().Be(_user.Email);
    }
}
