using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Infrastructure.Common.Persistence.Interceptors;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Infrastructure.UnitTests.Common.Persistence.Interceptors;

public class PersistDomainEventsToOutboxTests
{
    [Fact]
    public void Should_GenerateOutboxMessages_FromDomainEvents()
    {
        // Arrange
        var domainEvent = new TestDomainEvent(Guid.NewGuid());

        var entity = Substitute.For<IEntity>();
        entity.DomainEvents.Returns(new List<IDomainEvent> { domainEvent });

        var entities = new[] { entity };

        // Act
        var messages = PersistDomainEventsToOutbox.GenerateMessagesForTesting(entities);

        // Assert
        messages.Should().HaveCount(1);
        var message = messages.First();
        message.Type.Should().Be(nameof(TestDomainEvent));
        message.Payload.Should().Contain(domainEvent.Id.ToString());

        entity.Received(1).ClearDomainEvents();
    }

    private record TestDomainEvent(Guid Id) : IDomainEvent;
}
