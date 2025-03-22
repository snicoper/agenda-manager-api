using AgendaManager.Domain.Common.Messaging.Enums;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.Messaging.OutboxMessageAggregate;

public class OutboxMessageMarkAsProcessedTests
{
    [Fact]
    public void MarkAsProcessed_Should_Processed()
    {
        // Arrange
        var outbox = OutboxMessageFactory.CreateOutboxMessage();

        // Act
        outbox.MarkAsPublished();

        // Assert
        outbox.MessageStatus.Should().Be(OutboxMessageStatus.Published);
    }
}
