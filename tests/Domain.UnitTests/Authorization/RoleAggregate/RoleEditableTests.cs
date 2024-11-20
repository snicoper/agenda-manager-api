using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Authorization.RoleAggregate;

public class RoleEditableTests
{
    [Fact]
    public void RoleEditable_ShouldEditableStateBeFalse_WhenCreated()
    {
        // Arrange
        var role = RoleFactory.CreateRole();

        // Assert
        role.IsEditable.Should().BeFalse();
    }
}
