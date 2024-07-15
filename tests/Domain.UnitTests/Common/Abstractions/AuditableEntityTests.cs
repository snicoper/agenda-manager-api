using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Interfaces;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.Abstractions;

public class AuditableEntityTests
{
    [Fact]
    public void AuditableEntity_Should_BeAbstract()
    {
        // Act
        var result = typeof(AuditableEntity).IsAbstract;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void AuditableEntity_Should_BeSubclassOfEntity()
    {
        // Act
        var result = typeof(AuditableEntity).IsSubclassOf(typeof(Entity));

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void AuditableEntity_ShouldBeImplementingIAuditableEntity()
    {
        // Assert
        typeof(AuditableEntity).Should().Implement<IAuditableEntity>();
    }
}
