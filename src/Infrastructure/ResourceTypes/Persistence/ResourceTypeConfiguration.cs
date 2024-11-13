using AgendaManager.Domain.ResourceTypes;
using AgendaManager.Domain.ResourceTypes.ValueObjects;
using AgendaManager.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.ResourceTypes.Persistence;

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

        builder.Property(rt => rt.RoleId)
            .HasConversion(
                id => id != null ? id.Value : (Guid?)null,
                value => value != null ? RoleId.From((Guid)value) : null);

        builder.Property(rt => rt.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(rt => rt.Description)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasMany(rt => rt.Resources)
            .WithOne(r => r.Type)
            .HasForeignKey(r => r.TypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
