using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>

    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(x => x.CountryId);

            builder
                .Property(x => x.Name)
                .IsUnicode(true)
                .IsRequired(true)
                .HasMaxLength(60);
        }
    }
}
