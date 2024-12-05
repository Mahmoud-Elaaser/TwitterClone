using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TwitterClone.Data.Entities;

namespace TwitterClone.Infrastructure.Configurations
{
    public class TweetConfigurationb : IEntityTypeConfiguration<Tweet>
    {
        public void Configure(EntityTypeBuilder<Tweet> builder)
        {
            builder
                .HasKey(t => t.Id);


            builder
                .Property(c => c.Content)
                .HasMaxLength(280);


            builder
                .HasOne(t => t.User)
                .WithMany(u => u.Tweets)
                .HasForeignKey(t => t.UserId);

            builder
                .HasMany(t => t.Likes)
                .WithOne(l => l.Tweet)
                .HasForeignKey(l => l.TweetId);

            builder
                .HasMany(t => t.Comments)
                .WithOne(c => c.Tweet)
                .HasForeignKey(c => c.TweetId);

            builder
                .HasMany(t => t.Bookmarks)
                .WithOne(b => b.Tweet)
                .HasForeignKey(b => b.TweetId);

            builder
                .HasMany(t => t.Retweets)
                .WithOne(r => r.Tweet)
                .HasForeignKey(r => r.TweetId);

            builder.HasIndex(x => x.UserId);
        }
    }
}
