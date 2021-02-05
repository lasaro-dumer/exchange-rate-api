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
    public class CurrencyExchangeTransactionMap : IEntityTypeConfiguration<CurrencyExchangeTransaction>
    {
        public void Configure(EntityTypeBuilder<CurrencyExchangeTransaction> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                   .ValueGeneratedOnAdd()
                   .IsRequired();

            builder.Property(e => e.UserId)
                   .IsRequired();

            builder.Property(e => e.LocalCurrencyAmount)
                   .IsRequired();

            builder.Property(e => e.ForeignCurrencyCode)
                   .HasMaxLength(3)
                   .IsRequired();

            builder.Property(e => e.ForeignCurrencyAmount)
                   .IsRequired();

            builder.Property(e => e.TransactionDate)
                   .IsRequired();

            builder.HasOne(e => e.Rate)
                   .WithMany(r => r.Transactions)
                   .HasForeignKey(e => e.RateId)
                   .IsRequired();

        }
    }
}
