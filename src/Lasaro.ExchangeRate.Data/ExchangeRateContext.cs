using System;
using System.Collections.Generic;
using System.Text;
using Lasaro.ExchangeRate.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Lasaro.ExchangeRate.Data
{
    public class ExchangeRateContext : DbContext
    {
        public ExchangeRateContext(DbContextOptions options)
            : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RateMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
