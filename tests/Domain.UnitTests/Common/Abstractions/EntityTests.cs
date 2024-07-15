using AgendaManager.Domain.Common.Abstractions;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.Abstractions;

public class EntityTests
{
    [Fact]
    public void Entity_ShouldBeAbstract()
    {
        // Act
        var result = typeof(Entity).IsAbstract;

        // Assert
        result.Should().BeTrue();
    }
}
