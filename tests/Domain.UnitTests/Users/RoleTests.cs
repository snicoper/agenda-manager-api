using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users;

public class RoleTests
{
    [Fact]
    public void Role_ShouldReturnRole_WhenCreated()
    {
        // Act
        var role = RoleFactory.CreateRoleAdmin();

        // Assert
        role.Id.Should().Be(role.Id);
        role.Name.Should().Be(role.Name);
    }
}
