using AgendaManager.Domain.Users.Entities;
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
}
