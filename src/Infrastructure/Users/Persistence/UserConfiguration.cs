using AgendaManager.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Users.Persistence;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id.Value);

        builder.HasIndex(au => au.Email)
            .IsUnique();

        builder.HasIndex(au => au.RefreshToken)
            .IsUnique();

        builder.Property(au => au.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(au => au.FirstName)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(au => au.LastName)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(au => au.RefreshToken)
            .HasMaxLength(256);

        builder.Property(au => au.Active);

        builder.Property(au => au.RefreshTokenExpiryTime);
    }
}
