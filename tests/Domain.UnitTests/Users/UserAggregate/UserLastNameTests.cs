using AgendaManager.Domain.Users.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class UserLastNameTests
{
    [Fact]
    public void UserLastName_ShouldRaiseException_WhenInvalidLastNameIsSet()
    {
        // Arrange
        var lastName = new string('*', 257);

        // Act
        var action = () => UserFactory.CreateUser(lastName: lastName);

        // Assert
        action.Should().Throw<UserDomainException>();
        action.Should().Throw<UserDomainException>().WithMessage("Last name exceeds length of 256 characters.");
    }
}
