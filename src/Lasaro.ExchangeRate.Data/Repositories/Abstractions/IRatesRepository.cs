using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lasaro.ExchangeRate.Data.Entities;

namespace Lasaro.ExchangeRate.Data.Repositories.Abstractions
{
    public interface IRatesRepository
    {
        void AddRate(Rate rate);
        Task<List<Rate>> GetRatesAsync(string currencyCode = null, DateTime? effectiveDate = null);
        Task<int> SaveChangesAsync();
        Task<List<string>> GetAllCurrencyCodesOnRecordAsync();
    }
}
