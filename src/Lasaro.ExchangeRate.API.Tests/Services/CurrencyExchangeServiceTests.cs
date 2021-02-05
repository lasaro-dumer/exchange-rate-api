using System;
using System.Collections.Generic;
using System.Text;
using Lasaro.ExchangeRate.API.Models;
using Lasaro.ExchangeRate.API.Services.Implementations;
using Xunit;

namespace Lasaro.ExchangeRate.API.Tests.Services
{
    public class CurrencyExchangeServiceTests
    {
        [Theory]
        [InlineData(CurrencyExchangeDirection.BuyForeign, 1, 92.75, 86.75, 0.0107816711590297)]
        [InlineData(CurrencyExchangeDirection.BuyForeign, 86.75, 92.75, 86.75, 0.935309973045822)]
        [InlineData(CurrencyExchangeDirection.SellForeign, 1, 92.75, 86.75, 86.75)]
        [InlineData(CurrencyExchangeDirection.SellForeign, 0.0107816711590297, 92.75, 86.75, 0.935309973045822)]
        [InlineData(CurrencyExchangeDirection.BuyForeign, 1, 87.9196, 87.9196, 0.01137402809)]
        [InlineData(CurrencyExchangeDirection.BuyForeign, 87.9196, 87.9196, 87.9196, 1)]
        [InlineData(CurrencyExchangeDirection.SellForeign, 1, 87.9196, 87.9196, 87.9196)]
        [InlineData(CurrencyExchangeDirection.SellForeign, 0.01137402809, 87.9196, 87.9196, 1)]
        public void CalculateForeignCurrencyAmount_Should_Take_Direction_And_Different_Rates_Into_Consideration(CurrencyExchangeDirection direction, double localCurrencyAmount, double sellValue, double buyValue, double expectedAmount)
        {
            //Arrange
            CurrencyExchangeService currencyExchangeService = new CurrencyExchangeService();

            //Act
            double actualAmount = currencyExchangeService.CalculateForeignCurrencyAmount(direction, localCurrencyAmount, sellValue, buyValue);

            //Assert
            Assert.Equal(Math.Round(expectedAmount,5), Math.Round(actualAmount,5));
        }
    }
}
