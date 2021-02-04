using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lasaro.ExchangeRate.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lasaro.ExchangeRate.Data.Mappings
{
    public class RateMap : IEntityTypeConfiguration<Rate>
    {
        public void Configure(EntityTypeBuilder<Rate> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                   .ValueGeneratedOnAdd()
                   .IsRequired();

            builder.Property(e => e.CurrencyCode)
                   .HasMaxLength(3)
                   .IsRequired();

            builder.Property(e => e.EffectiveDate)
                   .IsRequired();

            builder.Property(e => e.BuyValue)
                   .IsRequired();

            builder.Property(e => e.SellValue)
                   .IsRequired();

            builder.Property(e => e.CreateDate)
                   .HasDefaultValueSql("GETDATE()")
                   .IsRequired();
        }
    }
}
