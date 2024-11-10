using AgendaManager.Domain.AuditRecords.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.AuditRecords.Aggregates.AuditAggregate;

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
}
