using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TwitterClone.Data.Entities;

namespace TwitterClone.Infrastructure.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder
                .HasOne(n => n.Sender)
                .WithMany(u => u.SentNotifications)
                .HasForeignKey(n => n.SenderId);

            builder
                .HasOne(n => n.Receiver)
                .WithMany(u => u.ReceivedNotifications)
                .HasForeignKey(n => n.ReceiverId);


            builder
                .Property(n => n.Type)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(n => n.Message)
                .HasMaxLength(255);

            builder
                .Property(n => n.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder
                .Property(n => n.RelatedEntityType)
                .HasConversion<string>(); // Stores the enum as a string

            builder.HasIndex(n => n.ReceiverId);
        }
    }

}


//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using TwitterClone.Data.Entities;

//namespace TwitterClone.Infrastructure.Configurations
//{
//    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
//    {
//        public void Configure(EntityTypeBuilder<Notification> builder)
//        {
//            builder.HasOne(n => n.Sender)
//                .WithMany(u => u.SentNotifications)
//                .HasForeignKey(n => n.SenderId)
//                .OnDelete(DeleteBehavior.NoAction);

//            builder.HasOne(n => n.Receiver)
//                .WithMany(u => u.ReceivedNotifications)
//                .HasForeignKey(n => n.ReceiverId)
//                .OnDelete(DeleteBehavior.Cascade);

//            builder.Property(n => n.Type)
//                .HasMaxLength(50)
//                .IsRequired();

//            builder.Property(n => n.Message)
//                .HasMaxLength(255);

//            builder.Property(n => n.CreatedAt)
//                .HasDefaultValueSql("GETDATE()");
//        }
//    }
//}
