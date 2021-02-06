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
    public class CurrencyMap : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                   .ValueGeneratedOnAdd()
                   .IsRequired();

            builder.Property(e => e.Code)
                   .HasMaxLength(3)
                   .IsRequired();

            builder.Property(e => e.MonthlyTransactionLimit)
                   .IsRequired();
        }
    }
}
