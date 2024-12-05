using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TwitterClone.Data.Entities;

namespace TwitterClone.Infrastructure.Configurations
{
    public class MuteUserConfiguration : IEntityTypeConfiguration<MuteUser>
    {
        public void Configure(EntityTypeBuilder<MuteUser> builder)
        {
            builder
                .HasOne(m => m.MutedBy)
                .WithMany(u => u.MutedUsers)
                .HasForeignKey(m => m.MutedById);

            builder
                .HasOne(m => m.MutedUser)
                .WithMany(u => u.MutedByUsers)
                .HasForeignKey(m => m.MutedUserId);


            builder.HasIndex(m => new { m.MutedById, m.MutedUserId }).IsUnique();
        }
    }

}