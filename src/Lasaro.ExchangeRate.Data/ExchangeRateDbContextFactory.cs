using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Lasaro.ExchangeRate.Data
{
    class ExchangeRateDbContextFactory : IDesignTimeDbContextFactory<ExchangeRateContext>
    {
        public ExchangeRateContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<ExchangeRateContext> optionsBuilder = new DbContextOptionsBuilder<ExchangeRateContext>();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json")
                            .AddEnvironmentVariables()
                            .AddUserSecrets("b888b77b-ee5f-4128-a2a6-010e5d27cd00")
                            //.AddCommandLine(args)
                            .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.EnableSensitiveDataLogging(true);
            optionsBuilder.EnableDetailedErrors(true);
            //optionsBuilder.UseSqlite("Data Source=blog.db");
            optionsBuilder.UseSqlServer(connectionString);

            return new ExchangeRateContext(optionsBuilder.Options);
        }
    }
}
