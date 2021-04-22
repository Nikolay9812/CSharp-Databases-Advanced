using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data.Configurations
{
    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.HasKey(x => x.PositionId);

            builder
                .Property(x => x.Name)
                .IsUnicode(true)
                .IsRequired(true)
                .HasMaxLength(100);
        }
    }
}
