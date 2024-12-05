using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TwitterClone.Data.Entities;

namespace TwitterClone.Infrastructure.Configurations
{
    public class RetweetConfiguration : IEntityTypeConfiguration<Retweet>
    {
        public void Configure(EntityTypeBuilder<Retweet> builder)
        {
            builder
                .HasOne(u => u.User)
                .WithMany(r => r.Retweets)
                .HasForeignKey(u => u.UserId);

            builder
                .HasOne(t => t.Tweet)
                .WithMany(r => r.Retweets)
                .HasForeignKey(t => t.TweetId);

            builder.HasIndex(t => t.TweetId);
            builder.HasIndex(t => t.UserId);
        }
    }
}
