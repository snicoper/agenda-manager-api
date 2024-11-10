using AgendaManager.Domain.AuditRecords;
using AgendaManager.Domain.AuditRecords.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.AuditRecords.Persistence;

public class AuditRecordConfiguration : IEntityTypeConfiguration<AuditRecord>
{
    public void Configure(EntityTypeBuilder<AuditRecord> builder)
    {
        builder.ToTable("AuditRecords");

        builder.HasKey(cl => cl.Id);

        builder.HasIndex(cl => cl.AggregateId);
        builder.HasIndex(cl => cl.AggregateName);
        builder.HasIndex(cl => new { cl.AggregateName, cl.AggregateId });

        builder.Property(cl => cl.Id)
            .HasConversion(
                id => id.Value,
                value => AuditRecordId.From(value))
            .IsRequired();

        builder.Property(cl => cl.AggregateId)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(cl => cl.NamespaceName)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(cl => cl.AggregateName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(cl => cl.PropertyName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(cl => cl.OldValue)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(cl => cl.NewValue)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(cl => cl.ActionType)
            .IsRequired();
    }
}
