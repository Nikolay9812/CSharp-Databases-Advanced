using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.UserId);

            builder
                .Property(x => x.Username)
                .IsUnicode(true)
                .IsRequired(true)
                .HasMaxLength(60);

            builder
                .Property(x => x.Email)
                .IsUnicode(true)
                .IsRequired(true)
                .HasMaxLength(100);

            builder
                .Property(x => x.Password)
                .IsUnicode(true)
                .IsRequired(true)
                .HasMaxLength(300);

            builder
                .Property(x => x.Name)
                .IsUnicode(true)
                .IsRequired(true)
                .HasMaxLength(100);
        }
    }
}
