using AgendaManager.Domain.Common.Abstractions;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.Abstractions;

public class EntityTests
{
    [Fact]
    public void Should_Be_Abstract()
    {
        // Act
        var result = typeof(Entity).IsAbstract;

        // Assert
        result.Should().BeTrue();
    }
}
