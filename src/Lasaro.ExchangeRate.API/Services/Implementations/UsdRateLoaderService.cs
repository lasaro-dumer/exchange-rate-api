using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Lasaro.ExchangeRate.API.Models;
using Lasaro.ExchangeRate.API.Services.Abstractions;
using Newtonsoft.Json;
using Serilog;

namespace Lasaro.ExchangeRate.API.Services.Implementations
{
    public class UsdRateLoaderService : ICurrencyRateLoaderService
    {
        public int Priority => 1;
        public string CurrencyCode => "USD";
        public IRatesService RatesService { get; }

        public UsdRateLoaderService(IRatesService ratesService)
        {
            RatesService = ratesService;
        }

        public async Task<CurrencyQuoteModel> LoadRateAsync()
        {
            Log.Information("Starting to load USD rate");

            CurrencyQuoteModel usdRate = null;

            using (HttpClient httpClient = new HttpClient())
            {
                string url = $"https://www.bancoprovincia.com.ar/Principal/Dolar";

                Log.Information("Performing HTTP Request");

                using (HttpResponseMessage responseApi = await httpClient.GetAsync(url))
                {
                    Log.Information("Response received");

                    string response = await responseApi.Content.ReadAsStringAsync();

                    if (responseApi.IsSuccessStatusCode)
                    {
                        Log.Information("Response success, parsing to object. Raw string: {response}", response);

                        string[] responseObj = JsonConvert.DeserializeObject<string[]>(response);

                        usdRate = new CurrencyQuoteModel()
                        {
                            CurrencyCode = "USD",
                            BuyValue = Convert.ToDouble(responseObj[0], new CultureInfo("en-US")),
                            SellValue = Convert.ToDouble(responseObj[1], new CultureInfo("en-US")),
                            EffectiveDate = DateTime.Now
                        };

                        Log.Information("Parsed rate: {usdRate}", usdRate);

                        await RatesService.AddRateAsync(usdRate);
                    }
                }
            }

            return usdRate;
        }
    }
}
