using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lasaro.ExchangeRate.API.Models;
using Lasaro.ExchangeRate.API.Services.Abstractions;
using Lasaro.ExchangeRate.Data.Entities;
using Lasaro.ExchangeRate.Data.Repositories.Abstractions;

namespace Lasaro.ExchangeRate.API.Services.Implementations
{
    public class RatesService : IRatesService
    {
        private IRatesRepository RatesRepository { get; }

        public RatesService(IRatesRepository ratesRepository)
        {
            RatesRepository = ratesRepository;
        }

        public async Task<bool> AddRateAsync(CurrencyQuoteModel currencyQuote)
        {
            Rate rate = new Rate()
            {
                CurrencyCode = currencyQuote.CurrencyCode,
                EffectiveDate = currencyQuote.EffectiveDate,
                BuyValue = currencyQuote.BuyValue,
                SellValue = currencyQuote.SellValue,
                CreateDate = DateTime.Now
            };

            RatesRepository.AddRate(rate);

            return (await RatesRepository.SaveChangesAsync()) > 0;
        }

        public async Task<bool> IsValidCurrencyToQuoteAsync(string currencyCode)
        {
            List<string> validCurrencies = await RatesRepository.GetAllCurrencyCodesOnRecordAsync();

            return validCurrencies.Contains(currencyCode);
        }

        public async Task<CurrencyQuoteModel> GetRateQuoteAsync(string currencyCode, DateTime date)
        {
            Rate rate = (await RatesRepository.GetRatesAsync(currencyCode, date))
                                              .FirstOrDefault();

            if (rate != null)
            {
                CurrencyQuoteModel currencyQuote = new CurrencyQuoteModel()
                {
                    CurrencyCode = rate.CurrencyCode,
                    EffectiveDate = rate.EffectiveDate,
                    BuyValue = rate.BuyValue,
                    SellValue = rate.SellValue
                };

                return currencyQuote;
            }

            return null;
        }

        public async Task<List<CurrencyQuoteModel>> GetRatesAsync(string currencyCode = null, DateTime? date = null)
        {
            List<CurrencyQuoteModel> currencyQuotes = new List<CurrencyQuoteModel>();

            List<Rate> rates = await RatesRepository.GetRatesAsync(currencyCode, date);

            foreach (Rate rate in rates)
            {
                CurrencyQuoteModel currencyQuote = new CurrencyQuoteModel()
                {
                    CurrencyCode = rate.CurrencyCode,
                    EffectiveDate = rate.EffectiveDate,
                    BuyValue = rate.BuyValue,
                    SellValue = rate.SellValue
                };

                currencyQuotes.Add(currencyQuote);
            }

            return currencyQuotes;
        }
    }
}
