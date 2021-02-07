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

        public void AddTransaction(CurrencyExchangeTransaction transaction)
        {
            _context.Add(transaction);
        }

        public async Task<double?> GetCurrencyTransactionLimitAsync(string currencyCode)
        {
            double? limit = (await _context.Set<Currency>()
                                           .Where(c => c.Code.ToUpper() == currencyCode.ToUpper())
                                           .FirstOrDefaultAsync())?.MonthlyTransactionLimit;
            
            return limit;
        }

        public async Task<List<CurrencyExchangeTransaction>> GetTransactionsAsync()
        {
            return await _context.Set<CurrencyExchangeTransaction>()
                                 .ToListAsync();
        }

        public async Task<double> GetUserAmountExchangedPerMonthAsync(int userId, string currencyCode, DateTime dateOfTheMonth)
        {
            double amount = await _context.Set<CurrencyExchangeTransaction>()
                                          .Where(ct => ct.UserId == userId &&
                                                       ct.ForeignCurrencyCode.ToUpper() == currencyCode.ToUpper() &&
                                                       ct.TransactionDate.Year == dateOfTheMonth.Year &&
                                                       ct.TransactionDate.Month == dateOfTheMonth.Month)
                                          .SumAsync(ct => ct.ForeignCurrencyAmount);

            return amount;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
