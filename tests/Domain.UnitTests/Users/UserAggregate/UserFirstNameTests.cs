using AgendaManager.Domain.Users.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class UserFirstNameTests
{
    [Fact]
    public void UserFirstName_ShouldRaiseException_WhenInvalidFirstNameIsSet()
    {
        // Arrange
        var firstName = new string('*', 257);

        // Act
        var action = () => UserFactory.CreateUser(firstName: firstName);

        // Assert
        action.Should().Throw<UserDomainException>();
        action.Should().Throw<UserDomainException>().WithMessage("First name exceeds length of 256 characters.");
    }
}
