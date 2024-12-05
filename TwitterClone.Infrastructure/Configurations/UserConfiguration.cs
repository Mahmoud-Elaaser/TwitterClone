using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TwitterClone.Data.Entities;

namespace TwitterClone.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder
                .Property(u => u.Bio)
                .HasMaxLength(160);


            builder
                .Property(u => u.DateJoined)
                .IsRequired();
        }
    }
}
