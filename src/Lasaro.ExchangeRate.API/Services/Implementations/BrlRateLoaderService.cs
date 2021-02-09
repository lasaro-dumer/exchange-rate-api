using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lasaro.ExchangeRate.API.Models;
using Lasaro.ExchangeRate.API.Services.Abstractions;
using Serilog;

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

            Log.Information("Loading BRL rates for {rateDate}", rateDate);
            Log.Information("Getting USD rate for {rateDate} to derivate BRL", rateDate);

            CurrencyQuoteModel usdRate = await RatesService.GetRateQuoteAsync("USD", rateDate);

            Log.Information("Retrieved USD rate: {usdRate}", usdRate);

            CurrencyQuoteModel brlRate = new CurrencyQuoteModel()
            {
                CurrencyCode = "BRL",
                BuyValue = usdRate.BuyValue / 4,
                SellValue = usdRate.SellValue / 4,
                EffectiveDate = rateDate
            };

            Log.Information("Derivated BRL rate: {brlRate}", brlRate);

            await RatesService.AddRateAsync(brlRate);

            return brlRate;
        }
    }
}
