using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lasaro.ExchangeRate.API.Models;
using Lasaro.ExchangeRate.API.Services.Implementations;
using Lasaro.ExchangeRate.Data.Repositories.Abstractions;
using Moq;
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
            Mock<ICurrencyExchangeTransactionsRepository> mock = new Mock<ICurrencyExchangeTransactionsRepository>();
            CurrencyExchangeService currencyExchangeService = new CurrencyExchangeService(mock.Object);

            //Act
            double actualAmount = currencyExchangeService.CalculateForeignCurrencyAmount(direction, localCurrencyAmount, sellValue, buyValue);

            //Assert
            Assert.Equal(Math.Round(expectedAmount, 5), Math.Round(actualAmount, 5));
        }

        [Theory]
        [InlineData("USD", 200, 0, 200)]
        [InlineData("USD", 200, 200, 0)]
        [InlineData("USD", 200, 150, 50)]
        [InlineData("USD", 200, 100, 100)]
        [InlineData("BRL", 300, 0, 300)]
        [InlineData("BRL", 300, 200, 100)]
        [InlineData("BRL", 300, 150, 150)]
        [InlineData("BRL", 300, 100, 200)]
        public async Task GetUserRemainingLimitAsync_Should_Consider_Multiple_Transactions_Per_MonthAsync(string currencyCode, double currencyLimit, double currentlyUsedQuota, double expectedRemaingLimit)
        {
            //Arrange
            int userId = 1;
            DateTime transactionDate = DateTime.Now;

            Mock<ICurrencyExchangeTransactionsRepository> mock = new Mock<ICurrencyExchangeTransactionsRepository>();

            mock.Setup(o => o.GetCurrencyTransactionLimitAsync(currencyCode))
                .ReturnsAsync(currencyLimit);

            mock.Setup(o => o.GetUserAmountExchangedPerMonthAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .ReturnsAsync(currentlyUsedQuota);
            CurrencyExchangeService currencyExchangeService = new CurrencyExchangeService(mock.Object);

            //Act
            double actualRemainingLimit = await currencyExchangeService.GetUserRemainingLimitAsync(userId, currencyCode, transactionDate);

            //Assert
            Assert.Equal(expectedRemaingLimit, actualRemainingLimit);
        }
    }
}
