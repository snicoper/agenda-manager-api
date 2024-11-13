using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.Enums;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.Entities.UserTokens;

public class UserTokenCreateTests
{
    private readonly UserToken _userToken = UserTokenFactory.CreateUserToken();

    [Fact]
    public void UserTokenCreate_ShouldReturnUserToken_WhenUserTokenIsCreated()
    {
        // Assert
        _userToken.Should().NotBeNull();
    }

    [Fact]
    public void UserTokenCreate_ShouldRaiseEvent_WhenUserTokenIsCreated()
    {
        // Assert
        _userToken.DomainEvents.Should().Contain(x => x is UserTokenCreatedDomainEvent);
    }

    [Fact]
    public void UserTokenCreate_ShouldReturnUserToken_WhenUserTokenIsCreatedWithEmailConfirmation()
    {
        // Act
        var userToken = UserToken.CreateEmailConfirmation(UserId.Create());
        var dayOfYearExpected = DateTimeOffset.UtcNow.AddDays(7).DayOfYear;

        // Assert
        userToken.Should().NotBeNull();
        userToken.Value?.Token.Should().NotBeNull();
        userToken.Value?.Type.Should().Be(UserTokenType.EmailConfirmation);
        userToken.Value?.IsExpired.Should().BeFalse();
        userToken.Value?.Token.Expires.DateTime.DayOfYear.Should().Be(dayOfYearExpected);
    }

    [Fact]
    public void UserTokenCreate_ShouldReturnUserToken_WhenUserTokenIsCreatedWithPasswordReset()
    {
        // Act
        var userToken = UserToken.CreatePasswordReset(UserId.Create());
        var dateExpected = DateTimeOffset.UtcNow.AddHours(1);
        var hourExpected = dateExpected.Hour;

        // Assert
        userToken.Should().NotBeNull();
        userToken.Value?.Token.Should().NotBeNull();
        userToken.Value?.Type.Should().Be(UserTokenType.PasswordReset);
        userToken.Value?.IsExpired.Should().BeFalse();
        userToken.Value?.Token.Expires.DateTime.Hour.Should().Be(hourExpected);
        userToken.Value?.Token.Expires.DateTime.Date.Should().Be(dateExpected.Date);
    }
}
