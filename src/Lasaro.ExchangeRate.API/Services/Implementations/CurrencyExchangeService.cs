using System;
using System.Threading.Tasks;
using Lasaro.ExchangeRate.API.Models;
using Lasaro.ExchangeRate.API.Services.Abstractions;

namespace Lasaro.ExchangeRate.API.Services.Implementations
{
    public class CurrencyExchangeService : ICurrencyExchangeService
    {
        public Task<double> GetUserRemainingLimitAsync(int userId, string currencyCode)
        {
            throw new System.NotImplementedException();
        }

        public double CalculateForeignCurrencyAmount(CurrencyExchangeDirection direction, double localCurrencyAmount, double sellValue, double buyValue)
        {
            double effectiveRate = 0;

            if (direction == CurrencyExchangeDirection.BuyForeign)
            {
                //Client if buying from the market, it will pay the value the market is selling
                effectiveRate = sellValue;
            }
            else if (direction == CurrencyExchangeDirection.SellForeign)
            {
                //Client is selling to the market, it will receive the value the market is buying
                effectiveRate = 1 / buyValue;
            }
            else
            {
                throw new NotImplementedException("Invalid exchange direction!");
            }

            double ForeignCurrencyAmount = localCurrencyAmount / effectiveRate; 

            return ForeignCurrencyAmount;
        }
    }
}
