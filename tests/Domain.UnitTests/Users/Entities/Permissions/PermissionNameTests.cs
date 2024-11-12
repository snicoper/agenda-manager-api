using AgendaManager.Domain.Users.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.Entities.Permissions;

public class PermissionNameTests
{
    [Theory]
    [InlineData("invalid name")]
    [InlineData("name:invalid")]
    public void PermissionName_ShouldThrowException_WhenNameSuffixAreInvalid(string invalidName)
    {
        // Act
        var action = () => PermissionFactory.CreatePermission(name: invalidName);

        // Arrange
        action.Should().Throw<PermissionDomainException>();
        action.Should()
            .Throw<PermissionDomainException>()
            .WithMessage("Permission name cannot end with ':create', ':update', ':delete', or ':read'.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public void PermissionName_ShouldRaiseException_WhenInvalidName(int nameLength)
    {
        // Arrange
        var name = new string('*', nameLength);

        // Act
        var action = () => PermissionFactory.CreatePermission(name: name);

        // Assert
        action.Should().Throw<PermissionDomainException>();
        action.Should()
            .Throw<PermissionDomainException>()
            .WithMessage("Permission name is null or exceeds length of 100 characters.");
    }
}
