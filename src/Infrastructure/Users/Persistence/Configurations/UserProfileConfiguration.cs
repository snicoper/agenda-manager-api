using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Users.Persistence.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("UserProfiles");

        builder.HasKey(up => up.Id);

        builder.Property(up => up.Id)
            .HasConversion(
                id => id.Value,
                value => UserProfileId.From(value))
            .IsRequired();

        builder.Property(up => up.UserId)
            .HasConversion(
                userId => userId.Value,
                value => UserId.From(value))
            .IsRequired();

        builder.HasOne(up => up.User)
            .WithOne(u => u.Profile)
            .HasForeignKey<User>(u => u.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(up => up.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(up => up.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.OwnsOne(
            up => up.PhoneNumber,
            phoneNumberBuilder =>
            {
                phoneNumberBuilder.Property(phoneNumber => phoneNumber.Number)
                    .HasColumnName(nameof(PhoneNumber))
                    .HasMaxLength(15)
                    .IsRequired();

                phoneNumberBuilder.Property(phoneNumber => phoneNumber.CountryCode)
                    .HasColumnName("PhoneCountryCode")
                    .HasMaxLength(4)
                    .IsRequired();
            });

        builder.OwnsOne(
            up => up.Address,
            addressBuilder =>
            {
                addressBuilder.Property(address => address.Street)
                    .HasColumnName(nameof(Address.Street))
                    .HasMaxLength(100)
                    .IsRequired();

                addressBuilder.Property(address => address.City)
                    .HasColumnName(nameof(Address.City))
                    .HasMaxLength(50)
                    .IsRequired();

                addressBuilder.Property(address => address.State)
                    .HasColumnName(nameof(Address.State))
                    .HasMaxLength(50)
                    .IsRequired();

                addressBuilder.Property(address => address.Country)
                    .HasColumnName(nameof(Address.Country))
                    .HasMaxLength(50)
                    .IsRequired();

                addressBuilder.Property(address => address.PostalCode)
                    .HasColumnName(nameof(Address.PostalCode))
                    .HasMaxLength(10)
                    .IsRequired();
            });

        builder.OwnsOne(
            up => up.IdentityDocument,
            identityDocumentBuilder =>
            {
                identityDocumentBuilder
                    .HasIndex(id => new { id.Value, id.CountryCode, id.Type })
                    .IsUnique();

                identityDocumentBuilder.Property(identityDocument => identityDocument.Value)
                    .HasColumnName("IdentityDocument")
                    .HasMaxLength(20)
                    .IsRequired();

                identityDocumentBuilder.Property(identityDocument => identityDocument.CountryCode)
                    .HasColumnName("IdentityDocumentCountryCode")
                    .HasMaxLength(2)
                    .IsRequired();

                identityDocumentBuilder.Property(identityDocument => identityDocument.Type)
                    .HasColumnName("IdentityDocumentType")
                    .IsRequired();
            });
    }
}
