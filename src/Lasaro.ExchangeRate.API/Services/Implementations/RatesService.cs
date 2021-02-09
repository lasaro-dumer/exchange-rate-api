using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lasaro.ExchangeRate.API.Models;
using Lasaro.ExchangeRate.API.Services.Abstractions;
using Lasaro.ExchangeRate.Data.Entities;
using Lasaro.ExchangeRate.Data.Repositories.Abstractions;
using Serilog;

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
            List<string> validCurrencies = await RatesRepository.GetAllCurrencyCodesAsync();

            return validCurrencies.Contains(currencyCode.ToUpper());
        }

        public async Task<CurrencyQuoteModel> GetRateQuoteAsync(string currencyCode, DateTime date)
        {
            Log.Information($"Searching for {currencyCode} rate for {date}");

            Rate rate = (await RatesRepository.GetRatesAsync(currencyCode, date))
                                              .FirstOrDefault();

            if (rate != null)
            {
                Log.Information($"Found rate");

                CurrencyQuoteModel currencyQuote = new CurrencyQuoteModel()
                {
                    RateId = rate.Id,
                    CurrencyCode = rate.CurrencyCode,
                    EffectiveDate = rate.EffectiveDate,
                    BuyValue = rate.BuyValue,
                    SellValue = rate.SellValue
                };

                return currencyQuote;
            }
            else
            {
                Log.Information($"No rate found");
            }

            return null;
        }

        private async Task<List<CurrencyQuoteModel>> GetRatesAsync(string currencyCode = null, DateTime? date = null, bool? onlyLatest = false)
        {
            List<CurrencyQuoteModel> currencyQuotes = new List<CurrencyQuoteModel>();

            Log.Information($"Searching for '{currencyCode}' rates for '{date}'");

            List<Rate> rates = await RatesRepository.GetRatesAsync(currencyCode, date);

            Log.Information($"Found '{rates.Count}' rates");

            foreach (Rate rate in rates)
            {
                bool skipCurrentRecord = onlyLatest.HasValue && onlyLatest.Value && currencyQuotes.Any(q => q.CurrencyCode == rate.CurrencyCode);
                if (!skipCurrentRecord)
                {
                    CurrencyQuoteModel currencyQuote = new CurrencyQuoteModel()
                    {
                        RateId = rate.Id,
                        CurrencyCode = rate.CurrencyCode,
                        EffectiveDate = rate.EffectiveDate,
                        BuyValue = rate.BuyValue,
                        SellValue = rate.SellValue
                    };

                    currencyQuotes.Add(currencyQuote);
                }
            }

            return currencyQuotes;
        }

        public async Task<List<CurrencyQuoteModel>> GetRatesAsync(string currencyCode = null, DateTime? date = null)
        {
            return await GetRatesAsync(currencyCode, date, onlyLatest: false);
        }

        public async Task<List<CurrencyQuoteModel>> GetLatestRatesAsync(string currencyCode = null, DateTime? date = null)
        {
            return await GetRatesAsync(currencyCode, date, onlyLatest: true);
        }
    }
}
