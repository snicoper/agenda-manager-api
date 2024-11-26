using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Users.Persistence.Configruations;

public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.ToTable("UserTokens");

        builder.HasKey(ut => ut.Id);

        builder.Property(ut => ut.Id)
            .HasConversion(
                id => id.Value,
                value => UserTokenId.From(value))
            .IsRequired();

        builder.Property(ut => ut.UserId)
            .HasConversion(
                userId => userId.Value,
                value => UserId.From(value))
            .IsRequired();

        builder.OwnsOne(
            userToken => userToken.Token,
            tokenBuilder =>
            {
                tokenBuilder.HasIndex(token => token.Value)
                    .IsUnique();

                tokenBuilder.Property(token => token.Value)
                    .HasColumnName("Token")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                tokenBuilder.Property(token => token.Expires)
                    .HasColumnName("Expires");
            });

        builder.Property(ut => ut.Type)
            .IsRequired();
    }
}
