using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lasaro.ExchangeRate.Data.Repositories.Abstractions;
using Lasaro.ExchangeRate.Data.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lasaro.ExchangeRate.Data
{

    public static class Extensions
    {
        public static IServiceCollection AddExchangeRateData(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddScoped<IRatesRepository, RatesRepository>();
            services.AddDbContext<ExchangeRateContext>(optionsBuilder =>
            {
                optionsBuilder.UseLazyLoadingProxies();
                optionsBuilder.EnableSensitiveDataLogging(true);
                optionsBuilder.EnableDetailedErrors(true);
                //optionsBuilder.UseSqlite("Data Source=blog.db");
                optionsBuilder.UseSqlServer(connectionString);
            });
            return services;
        }
    }
}
