using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Users;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(user => user.Id);

        builder.HasIndex(user => user.UserName)
            .IsUnique();

        builder.HasIndex(user => user.Email)
            .IsUnique();

        builder.Property(user => user.Id)
            .HasConversion(
                id => id.Value,
                id => UserId.From(id))
            .IsRequired();

        builder.Property(u => u.UserName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(user => user.Email)
            .HasConversion(
                email => email.Value,
                email => EmailAddress.From(email))
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(user => user.IsEmailConfirmed)
            .IsRequired();

        builder.Property(user => user.FirstName)
            .HasMaxLength(256);

        builder.Property(user => user.LastName)
            .HasMaxLength(256);

        builder.Property(user => user.Active)
            .IsRequired();

        builder.Property(user => user.PasswordHash)
            .IsRequired();

        builder.OwnsOne(
            user => user.RefreshToken,
            refreshTokenBuilder =>
            {
                refreshTokenBuilder.HasIndex(refreshToken => new { refreshToken.Token })
                    .IsUnique();

                refreshTokenBuilder.Property(refreshToken => refreshToken.Token)
                    .HasColumnName("RefreshTokenToken")
                    .HasMaxLength(256);

                refreshTokenBuilder.Property(refreshToken => refreshToken.ExpiryTime)
                    .HasColumnName("RefreshTokenExpiryTime");
            });
    }
}
