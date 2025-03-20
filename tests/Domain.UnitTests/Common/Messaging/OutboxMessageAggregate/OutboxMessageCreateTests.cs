using AgendaManager.Domain.Common.Messaging.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.Messaging.OutboxMessageAggregate;

public class OutboxMessageCreateTests
{
    [Fact]
    public void Create_ShouldCreated_WhenOutboxMessageIsProvided()
    {
        // Arrange
        var outbox = OutboxMessageFactory.CreateOutboxMessage();

        // Assert
        outbox.Should().NotBeNull();
    }

    [Fact]
    public void Create_ShouldRaiseException_WhenOutboxMessageIsProvidedWithInvalidType()
    {
        // Arrange
        var type = string.Empty;

        // Act
        var action = () => OutboxMessageFactory.CreateOutboxMessage(type: type);

        // Assert
        action.Should().Throw<OutboxMessageDomainException>()
            .WithMessage("Type cannot be empty.");
    }

    [Fact]
    public void Create_ShouldRaiseException_WhenOutboxMessageIsProvidedWithInvalidPayload()
    {
        // Arrange
        var payload = string.Empty;

        // Act
        var action = () => OutboxMessageFactory.CreateOutboxMessage(payload: payload);

        // Assert
        action.Should().Throw<OutboxMessageDomainException>()
            .WithMessage("Payload cannot be empty.");
    }
}
