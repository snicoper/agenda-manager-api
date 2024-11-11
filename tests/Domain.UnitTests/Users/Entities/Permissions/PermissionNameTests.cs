using AgendaManager.Domain.Users.Exceptions;
using AgendaManager.TestCommon.Factories;

namespace AgendaManager.Domain.UnitTests.Users.Entities.Permissions;

public class PermissionNameTests
{
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("invalid name")]
    [InlineData("name:invalid")]
    public void PermissionName_ShouldThrowException_WhenNameIsInvalid(string invalidName)
    {
        // Arrange
        Assert.Throws<PermissionDomainException>(() => PermissionFactory.CreatePermission(name: invalidName));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public void PermissionName_ShouldRaiseException_WhenInvalidNameIsSet(int nameLength)
    {
        // Arrange
        var name = new string('*', nameLength);

        // Assert
        Assert.Throws<PermissionDomainException>(() => PermissionFactory.CreatePermission(name: name));
    }
}
