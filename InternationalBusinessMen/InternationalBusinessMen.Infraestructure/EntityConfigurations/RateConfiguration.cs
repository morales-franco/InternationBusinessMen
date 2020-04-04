using InternationalBusinessMen.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternationalBusinessMen.Infraestructure.EntityConfigurations
{
    public class RateConfiguration : IEntityTypeConfiguration<Rate>
    {
        public void Configure(EntityTypeBuilder<Rate> builder)
        {
            builder
               .HasKey(u => u.Id);

            builder
               .Property(u => u.From)
               .IsRequired()
               .HasMaxLength(512);

            builder
               .Property(u => u.To)
               .IsRequired()
               .HasMaxLength(512);

            builder
                .Property(u => u.RateValue)
                .IsRequired()
                .HasColumnType("decimal(10, 5)");
        }
    }
}
