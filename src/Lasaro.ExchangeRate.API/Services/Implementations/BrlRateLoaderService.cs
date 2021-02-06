using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lasaro.ExchangeRate.API.Models;
using Lasaro.ExchangeRate.API.Services.Abstractions;

namespace Lasaro.ExchangeRate.API.Services.Implementations
{
    public class BrlRateLoaderService : ICurrencyRateLoaderService
    {
        public int Priority => 5;
        public string CurrencyCode => "BRL";
        public IRatesService RatesService { get; }

        public BrlRateLoaderService(IRatesService ratesService)
        {
            RatesService = ratesService;
        }

        public async Task<CurrencyQuoteModel> LoadRateAsync()
        {
            DateTime rateDate = DateTime.Now;
            CurrencyQuoteModel usdRate = await RatesService.GetRateQuoteAsync("USD", rateDate);

            CurrencyQuoteModel brlRate = new CurrencyQuoteModel()
            {
                CurrencyCode = "BRL",
                BuyValue = usdRate.BuyValue / 4,
                SellValue = usdRate.SellValue / 4,
                EffectiveDate = rateDate
            };

            await RatesService.AddRateAsync(brlRate);

            return brlRate;
        }
    }
}
