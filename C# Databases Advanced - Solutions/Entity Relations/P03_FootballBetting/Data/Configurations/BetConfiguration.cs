using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data.Configurations
{
    public class BetConfiguration : IEntityTypeConfiguration<Bet>
    {
        public void Configure(EntityTypeBuilder<Bet> builder)
        {
            builder.HasKey(x => x.BetId);

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.Bets)
                .HasForeignKey(x => x.BetId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Game)
                .WithMany(x => x.Bets)
                .HasForeignKey(x => x.BetId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
