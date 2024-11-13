using AgendaManager.Domain.AuditRecords;
using AgendaManager.Domain.AuditRecords.Enums;
using AgendaManager.Domain.AuditRecords.Events;
using AgendaManager.Domain.AuditRecords.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.AuditRecords.AuditAggregate;

public class AuditRecordCreateTests
{
    private readonly AuditRecord _auditRecord = AuditRecordFactory.Create();

    [Fact]
    public void AuditRecord_ShouldCreateAuditRecord_WhenCreateWithValidParametersAreProvided()
    {
        // Assert
        _auditRecord.Should().NotBeNull();
        _auditRecord.Id.Should().NotBeNull();
    }

    [Fact]
    public void AuditRecord_ShouldRaiseEvent_WhenCreateWithValidParametersAreProvided()
    {
        // Assert
        _auditRecord.DomainEvents.Should().NotBeNull();
        _auditRecord.DomainEvents.Should().Contain(x => x is AuditRecordCreatedDomainEvent);
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
