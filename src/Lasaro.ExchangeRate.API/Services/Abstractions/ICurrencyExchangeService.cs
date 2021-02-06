using System;
using System.Threading.Tasks;
using Lasaro.ExchangeRate.API.Models;

namespace Lasaro.ExchangeRate.API.Services.Abstractions
{
    public interface ICurrencyExchangeService
    {
        double CalculateForeignCurrencyAmount(CurrencyExchangeDirection direction, double localCurrencyAmount, double sellValue, double buyValue);
        Task<double> GetUserRemainingLimitAsync(int userId, string currencyCode, DateTime transactionDate);
    }
}
