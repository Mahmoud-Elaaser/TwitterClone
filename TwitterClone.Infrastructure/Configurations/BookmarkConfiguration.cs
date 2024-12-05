using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TwitterClone.Data.Entities;

namespace TwitterClone.Infrastructure.Configurations
{
    public class BookmarkConfiguration : IEntityTypeConfiguration<Bookmark>
    {
        public void Configure(EntityTypeBuilder<Bookmark> builder)
        {
            builder
                .HasOne(u => u.User)
                .WithMany(b => b.Bookmarks)
                .HasForeignKey(u => u.UserId);

            builder
                .HasOne(t => t.Tweet)
                .WithMany(b => b.Bookmarks)
                .HasForeignKey(t => t.TweetId);


            builder.HasIndex(u => u.UserId);
            builder.HasIndex(t => t.TweetId);
        }
    }
}