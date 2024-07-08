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

        builder.Property(user => user.FirstName)
            .HasMaxLength(256);

        builder.Property(u => u.LastName)
            .HasMaxLength(256);
    }
}
