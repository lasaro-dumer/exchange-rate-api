using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lasaro.ExchangeRate.API.Models;
using Lasaro.ExchangeRate.Data.Repositories.Abstractions;

namespace Lasaro.ExchangeRate.API.Services.Abstractions
{
    public interface IRatesService
    {
        Task<bool> AddRateAsync(CurrencyQuoteModel currencyQuote);
        Task<CurrencyQuoteModel> GetRateQuoteAsync(string currencyCode, DateTime date);
        Task<List<CurrencyQuoteModel>> GetRatesAsync(string currencyCode = null, DateTime? date = null);
        Task<List<CurrencyQuoteModel>> GetLatestRatesAsync(string currencyCode = null, DateTime? date = null);
        Task<bool> IsValidCurrencyToQuoteAsync(string currencyCode);
    }
}
