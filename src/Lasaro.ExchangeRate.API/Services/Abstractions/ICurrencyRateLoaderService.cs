using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lasaro.ExchangeRate.API.Models;

namespace Lasaro.ExchangeRate.API.Services.Abstractions
{
    public interface ICurrencyRateLoaderService
    {
        int Priority { get; }
        string CurrencyCode { get; }
        Task<CurrencyQuoteModel> LoadRateAsync();
    }
}
