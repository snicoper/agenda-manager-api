using AgendaManager.Domain.Resources.Aggregates;
using AgendaManager.Domain.Resources.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Resources.Persistence;

public class ResourceTypeConfiguration : IEntityTypeConfiguration<ResourceType>
{
    public void Configure(EntityTypeBuilder<ResourceType> builder)
    {
        builder.ToTable("ResourceTypes");

        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Id)
            .HasConversion(
                id => id.Value,
                value => ResourceTypeId.From(value))
            .IsRequired();

        builder.Property(rt => rt.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(rt => rt.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(rt => rt.RequiredRole);

        builder.HasMany(rt => rt.Resources)
            .WithOne(r => r.Type)
            .HasForeignKey(r => r.TypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
