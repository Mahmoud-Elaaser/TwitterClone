using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TwitterClone.Data.Entities;

namespace TwitterClone.Infrastructure.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder
                .Property(c => c.Content)
                .HasMaxLength(280)
                .IsRequired();

            builder
                .HasOne(c => c.Tweet)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TweetId);


            builder
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId);


            builder.HasIndex(t => t.TweetId);
            builder.HasIndex(t => t.UserId);
        }
    }
}