using LeadLane.Domain.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeadLane.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Login)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.PasswordHash)
            .HasMaxLength(512)
            .IsRequired();

        builder.HasIndex(x => x.Login)
            .IsUnique();
    }
}
