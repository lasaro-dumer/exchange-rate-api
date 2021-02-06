using System;
using System.Threading.Tasks;

namespace Lasaro.ExchangeRate.Data.Repositories.Abstractions
{
    public interface ICurrencyExchangeTransactionsRepository
    {
        Task<double> GetUserAmountExchangedPerMonthAsync(int userId, string currencyCode, DateTime dateOfTheMonth);
        Task<double?> GetCurrencyTransactionLimitAsync(string currencyCode);
    }
}
