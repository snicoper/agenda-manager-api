using AgendaManager.Domain.Common.Messaging;
using AgendaManager.Domain.Common.Messaging.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Common.Messaging.Persistence;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessages");

        builder.HasKey(om => om.Id);

        builder.Property(om => om.Id)
            .HasConversion(
                id => id.Value,
                value => OutboxMessageId.From(value));

        builder.Property(om => om.OccurredOn)
            .IsRequired();

        builder.Property(om => om.Type)
            .IsRequired();

        builder.Property(om => om.Payload)
            .IsRequired();

        builder.Property(om => om.MessageStatus)
            .IsRequired();

        builder.Property(om => om.PublishedOn)
            .HasColumnType("text");

        builder.Property(om => om.Error)
            .HasColumnType("text");

        builder.Property(om => om.RetryCount)
            .IsRequired();

        builder.Property(om => om.LastAttemptOn);
    }
}
