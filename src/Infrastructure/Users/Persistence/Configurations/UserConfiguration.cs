using AgendaManager.Domain.Common.ValueObjects.EmailAddress;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Users.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasConversion(
                id => id.Value,
                value => UserId.From(value))
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .HasConversion(
                password => password.HashedValue,
                value => PasswordHash.FromHashed(value))
            .IsRequired();

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.Email)
            .HasConversion(
                email => email.Value,
                value => EmailAddress.From(value))
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

        builder.OwnsOne(
            user => user.RefreshToken,
            refreshTokenBuilder =>
            {
                refreshTokenBuilder.HasIndex(rt => rt.Token)
                    .IsUnique();

                refreshTokenBuilder.Property(rt => rt.Token)
                    .HasColumnName("RefreshTokenToken")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                refreshTokenBuilder.Property(rt => rt.Expires)
                    .HasColumnName("RefreshTokenExpires");
            });

        builder.HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity(
                typeBuilder =>
                {
                    typeBuilder.ToTable("UserRoles");
                    typeBuilder.Property<UserId>("UserId").HasColumnName("UserId");
                    typeBuilder.Property<RoleId>("RoleId").HasColumnName("RoleId");
                    typeBuilder.HasKey("UserId", "RoleId");

                    // Campos de auditoría.
                    typeBuilder.Property<DateTimeOffset>("CreatedAt");
                    typeBuilder.Property<string>("CreatedBy");
                    typeBuilder.Property<DateTimeOffset>("LastModifiedAt");
                    typeBuilder.Property<string>("LastModifiedBy");
                });
    }
}
