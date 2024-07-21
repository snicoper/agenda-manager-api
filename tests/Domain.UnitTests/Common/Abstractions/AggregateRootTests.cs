using AgendaManager.Domain.Common.Abstractions;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.Abstractions;

public class AggregateRootTests
{
    [Fact]
    public void AggregateRoot_Should_BeAbstract()
    {
        // Act
        var result = typeof(AggregateRoot).IsAbstract;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void AggregateRoot_Should_BeSubclassOfAuditableEntity()
    {
        // Act
        var result = typeof(AggregateRoot).IsSubclassOf(typeof(Entity));

        // Assert
        result.Should().BeTrue();
    }
}
