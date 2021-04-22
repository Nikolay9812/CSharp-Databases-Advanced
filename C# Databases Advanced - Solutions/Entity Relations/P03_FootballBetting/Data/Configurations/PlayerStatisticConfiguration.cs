using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data.Configurations
{
    public class PlayerStatisticConfiguration : IEntityTypeConfiguration<PlayerStatistic>
    {
        public void Configure(EntityTypeBuilder<PlayerStatistic> builder)
        {
            builder.HasKey(x => new { x.GameId, x.PlayerId });

            builder
                .HasOne(x => x.Game)
                .WithMany(x => x.PlayerStatistics)
                .HasForeignKey(x => x.GameId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Player)
                .WithMany(x => x.PlayerStatistics)
                .HasForeignKey(x => x.PlayerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
