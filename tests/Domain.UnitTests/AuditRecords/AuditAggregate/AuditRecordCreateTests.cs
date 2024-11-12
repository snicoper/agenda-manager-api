using AgendaManager.Domain.AuditRecords.Enums;
using AgendaManager.Domain.AuditRecords.Events;
using AgendaManager.Domain.AuditRecords.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.AuditRecords.AuditAggregate;

public class AuditRecordCreateTests
{
    [Fact]
    public void AuditRecord_ShouldCreateAuditRecord_WhenCreateWithValidParametersAreProvided()
    {
        // Arrange
        var auditRecord = AuditRecordFactory.Create();

        // Assert
        auditRecord.Should().NotBeNull();
        auditRecord.Id.Should().NotBeNull();
    }

    [Fact]
    public void AuditRecord_ShouldRaiseEvent_WhenCreateWithValidParametersAreProvided()
    {
        // Arrange
        var auditRecord = AuditRecordFactory.Create();

        // Assert
        auditRecord.DomainEvents.Should().NotBeNull();
        auditRecord.DomainEvents.Should().Contain(x => x is AuditRecordCreatedDomainEvent);
    }

    [Fact]
    public void AuditRecord_ShouldRaiseException_WhenActionTypeIsInvalid()
    {
        // Arrange
        var action = () => AuditRecordFactory.Create(actionType: (ActionType)100);

        // Assert
        action.Should().Throw<AuditRecordDomainException>();
        action.Should().Throw<AuditRecordDomainException>().WithMessage("Invalid action type.");
    }

    [Fact]
    public void AuditRecord_ShouldRaiseException_WhenActionTypeIsNone()
    {
        // Act
        var action = () => AuditRecordFactory.Create(actionType: ActionType.None);

        // Assert
        action.Should().Throw<AuditRecordDomainException>();
        action.Should().Throw<AuditRecordDomainException>().WithMessage("Action type cannot be None.");
    }
}
