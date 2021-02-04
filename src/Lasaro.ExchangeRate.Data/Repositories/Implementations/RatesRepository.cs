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
    public class RatesRepository : IRatesRepository
    {
        private ExchangeRateContext _context { get; }

        public RatesRepository(ExchangeRateContext context)
        {
            _context = context;
        }

        public void AddRate(Rate rate)
        {
            _context.Add(rate);
        }

        public async Task<List<Rate>> GetRatesAsync(string currencyCode = null, DateTime? effectiveDate = null)
        {
            IQueryable<Rate> ratesQuery = _context.Set<Rate>().AsQueryable();

            if (!string.IsNullOrEmpty(currencyCode))
                ratesQuery = ratesQuery.Where(r => r.CurrencyCode == currencyCode);

            if (effectiveDate.HasValue)
                ratesQuery = ratesQuery.Where(r => r.EffectiveDate == effectiveDate);

            return await ratesQuery.OrderByDescending(r => r.CreateDate).ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<List<string>> GetAllCurrencyCodesOnRecordAsync()
        {
            return await _context.Set<Rate>().Select(r => r.CurrencyCode).ToListAsync();
        }
    }
}
