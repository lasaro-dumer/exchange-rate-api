using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Lasaro.ExchangeRate.API.Models;
using Lasaro.ExchangeRate.API.Services.Abstractions;
using Newtonsoft.Json;

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
            CurrencyQuoteModel usdRate = null;

            using (HttpClient httpClient = new HttpClient())
            {
                string url = $"https://www.bancoprovincia.com.ar/Principal/Dolar";

                using (HttpResponseMessage responseApi = await httpClient.GetAsync(url))
                {
                    string response = await responseApi.Content.ReadAsStringAsync();
                    if (responseApi.IsSuccessStatusCode)
                    {
                        string[] responseObj = JsonConvert.DeserializeObject<string[]>(response);

                        usdRate = new CurrencyQuoteModel()
                        {
                            CurrencyCode = "USD",
                            BuyValue = Convert.ToDouble(responseObj[0].Replace('.', ',')),
                            SellValue = Convert.ToDouble(responseObj[1].Replace('.', ',')),
                            EffectiveDate = DateTime.Now
                        };

                        await RatesService.AddRateAsync(usdRate);
                    }
                }
            }

            return usdRate;
        }
    }
}
