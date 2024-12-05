using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TwitterClone.Data.Entities;

namespace TwitterClone.Infrastructure.Configurations
{
    public class QuoteConfiguration : IEntityTypeConfiguration<Quote>
    {
        public void Configure(EntityTypeBuilder<Quote> builder)
        {

            builder
                .Property(q => q.Content)
                .HasMaxLength(400);


            builder
                .HasOne(q => q.User)
                .WithMany(u => u.Quotes)
                .HasForeignKey(q => q.UserId);


            builder
                .HasOne(t => t.Tweet)
                .WithMany(q => q.Quotes)
                .HasForeignKey(t => t.TweetId);


            builder.HasIndex(q => q.UserId);
            builder.HasIndex(q => q.TweetId);

        }
    }
}
