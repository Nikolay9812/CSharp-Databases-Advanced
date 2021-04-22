using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(x => x.TeamId);

            builder
                .Property(x => x.Name)
                .IsUnicode(true)
                .IsRequired(true)
                .HasMaxLength(50);

            builder
                .Property(x => x.LogoUrl)
                .IsUnicode(false)
                .IsRequired(true);

            builder
                .Property(x => x.Initials)
                .IsUnicode(true)
                .IsRequired(true)
                .HasMaxLength(3);

            builder
                .HasOne(x => x.PrimaryKitColor)
                .WithMany(x => x.PrimaryKitTeams)
                .HasForeignKey(x => x.PrimaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.SecondaryKitColor)
                .WithMany(x => x.SecondaryKitTeams)
                .HasForeignKey(x => x.SecondaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Town)
                .WithMany(x => x.Teams)
                .HasForeignKey(x => x.TownId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
