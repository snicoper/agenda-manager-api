using AgendaManager.Domain.Authorization.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Authorization.RoleAggregate;

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
        var action = () => role.Update(validName, invalidDescription);

        // Assert
        action.Should().Throw<RoleDomainException>();
    }
}
