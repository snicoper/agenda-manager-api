using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Users.Persistence;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity(
                j =>
                {
                    j.ToTable("UserRoles");
                    j.Property<UserId>("UserId").HasColumnName("UserId");
                    j.Property<RoleId>("RoleId").HasColumnName("RoleId");
                    j.HasKey("UserId", "RoleId");

                    // Campos de auditoría.
                    j.Property<DateTimeOffset>("CreatedAt");
                    j.Property<string>("CreatedBy");
                    j.Property<DateTimeOffset>("LastModifiedAt");
                    j.Property<string>("LastModifiedBy");
                });

        builder.Property(u => u.Id)
            .HasConversion(
                id => id.Value,
                id => UserId.From(id))
            .IsRequired();

        builder.Property(u => u.Email)
            .HasConversion(
                email => email.Value,
                email => EmailAddress.From(email))
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(u => u.IsEmailConfirmed)
            .IsRequired();

        builder.Property(u => u.FirstName)
            .HasMaxLength(256);

        builder.Property(u => u.LastName)
            .HasMaxLength(256);

        builder.Property(u => u.Active)
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.OwnsOne(
            user => user.RefreshToken,
            refreshTokenBuilder =>
            {
                refreshTokenBuilder.Property(rt => rt.Token)
                    .HasColumnName("RefreshTokenToken")
                    .HasMaxLength(200);

                refreshTokenBuilder.Property(rt => rt.ExpiryTime)
                    .HasColumnName("RefreshTokenExpiryTime");
            });
    }
}
