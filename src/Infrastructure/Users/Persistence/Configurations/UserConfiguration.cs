﻿using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Entities;
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

        builder.Property(u => u.ProfileId)
            .HasConversion(
                id => id.Value,
                value => UserProfileId.From(value))
            .IsRequired();

        builder.HasOne(u => u.Profile)
            .WithOne(up => up.User)
            .HasForeignKey<UserProfile>(up => up.Id)
            .OnDelete(DeleteBehavior.Cascade);

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

        builder.Property(u => u.IsActive)
            .IsRequired();

        builder.OwnsOne(
            user => user.RefreshToken,
            refreshTokenBuilder =>
            {
                refreshTokenBuilder.HasIndex(rt => rt.Value)
                    .IsUnique();

                refreshTokenBuilder.Property(rt => rt.Value)
                    .HasColumnName(nameof(User.RefreshToken))
                    .HasMaxLength(200)
                    .IsUnicode(false);

                refreshTokenBuilder.Property(rt => rt.Expires)
                    .HasColumnName("RefreshTokenExpires");
            });

        builder.HasMany(x => x.UserRoles)
            .WithOne()
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(u => u.Tokens)
            .HasField("_userTokens")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(u => u.Tokens)
            .WithOne()
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
