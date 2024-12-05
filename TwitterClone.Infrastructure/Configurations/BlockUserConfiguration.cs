using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TwitterClone.Data.Entities;

namespace TwitterClone.Infrastructure.Configurations
{
    public class BlockUserConfiguration : IEntityTypeConfiguration<BlockUser>
    {
        public void Configure(EntityTypeBuilder<BlockUser> builder)
        {
            builder
                .HasOne(b => b.BlockedBy)
                .WithMany(u => u.BlockedUsers)
                .HasForeignKey(b => b.BlockedById);

            builder
                .HasOne(b => b.BlockedUser)
                .WithMany(u => u.BlockedByUsers)
                .HasForeignKey(b => b.BlockedUserId);

            builder.HasIndex(b => new { b.BlockedById, b.BlockedUserId }).IsUnique();
        }
    }

}