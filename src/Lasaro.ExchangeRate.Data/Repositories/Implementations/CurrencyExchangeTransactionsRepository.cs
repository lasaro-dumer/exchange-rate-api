using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lasaro.ExchangeRate.Data.Entities;
using Lasaro.ExchangeRate.Data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Lasaro.ExchangeRate.Data.Repositories.Implementations
{
    public class CurrencyExchangeTransactionsRepository : ICurrencyExchangeTransactionsRepository
    {
        private ExchangeRateContext _context;

        public CurrencyExchangeTransactionsRepository(ExchangeRateContext context)
        {
            _context = context;
        }

        public async Task<double?> GetCurrencyTransactionLimitAsync(string currencyCode)
        {
            double? limit = (await _context.Set<Currency>()
                                           .Where(c => c.Code == currencyCode)
                                           .FirstOrDefaultAsync())?.MonthlyTransactionLimit;
            
            return limit;
        }

        public async Task<double> GetUserAmountExchangedPerMonthAsync(int userId, string currencyCode, DateTime dateOfTheMonth)
        {
            double amount = await _context.Set<CurrencyExchangeTransaction>()
                                          .Where(ct => ct.UserId == userId &&
                                                       ct.ForeignCurrencyCode == currencyCode &&
                                                       ct.TransactionDate.Year == dateOfTheMonth.Year &&
                                                       ct.TransactionDate.Month == dateOfTheMonth.Month)
                                          .SumAsync(ct => ct.ForeignCurrencyAmount);

            return amount;
        }
    }
}
