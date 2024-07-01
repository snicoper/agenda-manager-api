using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Interfaces;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.Abstractions;

public class AuditableEntityTests
{
    [Fact]
    public void Should_Be_Abstract()
    {
        // Act
        var result = typeof(AuditableEntity).IsAbstract;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Be_Subclass_Of_Entity()
    {
        // Act
        var result = typeof(AuditableEntity).IsSubclassOf(typeof(Entity));

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Be_Implementing_IAuditableEntity()
    {
        // Assert
        typeof(AuditableEntity).Should().Implement<IAuditableEntity>();
    }
}
