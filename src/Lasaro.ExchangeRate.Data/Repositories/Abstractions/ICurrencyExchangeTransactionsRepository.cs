using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lasaro.ExchangeRate.Data.Entities;

namespace Lasaro.ExchangeRate.Data.Repositories.Abstractions
{
    public interface ICurrencyExchangeTransactionsRepository
    {
        Task<double> GetUserAmountExchangedPerMonthAsync(int userId, string currencyCode, DateTime dateOfTheMonth);
        Task<double?> GetCurrencyTransactionLimitAsync(string currencyCode);
        void AddTransaction(CurrencyExchangeTransaction transaction);
        Task<int> SaveChangesAsync();
        Task<List<CurrencyExchangeTransaction>> GetTransactionsAsync();
    }
}
