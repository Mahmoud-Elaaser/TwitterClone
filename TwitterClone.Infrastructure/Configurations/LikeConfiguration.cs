﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TwitterClone.Data.Entities;

namespace TwitterClone.Infrastructure.Configurations
{
    public class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder
                .HasOne(l => l.Tweet)
                .WithMany(t => t.Likes)
                .HasForeignKey(l => l.TweetId);


            builder
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId);


            builder.HasIndex(l => l.TweetId);
            builder.HasIndex(l => l.UserId);

        }
    }

}