using AgendaManager.Domain.Users.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.Aggregates.RoleAggregate;

public class RoleDescriptionTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(501)]
    public void RoleDescription_ShouldThrowException_WhenDescriptionIsInvalid(int descriptionLength)
    {
        // Arrange
        var role = RoleFactory.CreateRole();
        const string validName = "Valid Name";
        var invalidDescription = new string('*', descriptionLength);

        // Act
        var act = () => role.UpdateRole(validName, invalidDescription);

        // Assert
        act.Should().Throw<RoleDomainException>();
    }
}
