using InternationalBusinessMen.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace InternationalBusinessMen.Infraestructure.EntityConfigurations
{
    class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder
               .HasKey(u => u.Id);

            builder
               .Property(u => u.Amount)
               .HasColumnType("decimal(10, 5)")
               .IsRequired();

            builder
               .Property(u => u.Currency)
               .IsRequired()
               .HasMaxLength(512);

            builder
              .Property(u => u.Sku)
              .IsRequired()
              .HasMaxLength(512);
        }
    }
}
