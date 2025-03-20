using AgendaManager.Domain.Common.Messaging.Enums;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.Messaging.OutboxMessageAggregate;

public class OutboxMessageMarkAsFailedTests
{
    [Fact]
    public void MarkAsProcessed_Should_MarkOutboxMessageAsFailed()
    {
        // Arrange
        var outbox = OutboxMessageFactory.CreateOutboxMessage();

        // Act
        outbox.MarkAsFailed("error test");

        // Assert
        outbox.MessageStatus.Should().Be(OutboxMessageStatus.Failed);
    }
}
