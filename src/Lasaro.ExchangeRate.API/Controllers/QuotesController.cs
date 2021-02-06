using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Lasaro.ExchangeRate.API.Models;
using Lasaro.ExchangeRate.API.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Lasaro.ExchangeRate.API.Controllers
{
    [ApiController]
    [Route("quotes")]
    public class QuotesController : ControllerBase
    {
        public IRatesService RatesService { get; }
        
        public QuotesController(IRatesService ratesService)
        {
            RatesService = ratesService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllQuotesAsync(DateTime? date = null)
        {
            return Ok(await RatesService.GetRatesAsync(date: date));
        }

        [HttpGet("currency/{currencyCode}")]
        public async Task<IActionResult> GetAsync(string currencyCode, DateTime? date = null)
        {
            DateTime effectiveDate = DateTime.Now.Date;

            if (date.HasValue)
                effectiveDate = date.Value.Date;

            if (!await RatesService.IsValidCurrencyToQuoteAsync(currencyCode))
                return BadRequest("Currency not valid");

            CurrencyQuoteModel currencyQuote = await RatesService.GetRateQuoteAsync(currencyCode, effectiveDate);

            if (currencyQuote == null)
                return NotFound($"No quote found for {currencyCode} on {effectiveDate}");

            return Ok(currencyQuote);
        }

        [HttpPost("loader")]
        public async Task<IActionResult> LoadCurrencyRates()
        {
            List<CurrencyQuoteModel> ratesLoaded = new List<CurrencyQuoteModel>();

            using (HttpClient httpClient = new HttpClient())
            {
                string url = $"https://www.bancoprovincia.com.ar/Principal/Dolar";

                using (HttpResponseMessage responseApi = await httpClient.GetAsync(url))
                {
                    string response = await responseApi.Content.ReadAsStringAsync();
                    if (responseApi.IsSuccessStatusCode)
                    {
                        string[] responseObj = JsonConvert.DeserializeObject<string[]>(response);

                        CurrencyQuoteModel usdRate = new CurrencyQuoteModel()
                        {
                            CurrencyCode = "USD",
                            BuyValue = Convert.ToDouble(responseObj[0]),
                            SellValue = Convert.ToDouble(responseObj[1]),
                            EffectiveDate = DateTime.Now
                        };

                        CurrencyQuoteModel brlRate = new CurrencyQuoteModel()
                        {
                            CurrencyCode = "BRL",
                            BuyValue = usdRate.BuyValue / 4,
                            SellValue = usdRate.SellValue / 4,
                            EffectiveDate = DateTime.Now
                        };

                        await RatesService.AddRateAsync(usdRate);
                        await RatesService.AddRateAsync(brlRate);

                        ratesLoaded.Add(usdRate);
                        ratesLoaded.Add(brlRate);
                    }
                    else
                    {
                        return BadRequest("Error fetching response from Banco Provincia, request failed.");
                    }
                }
            }

            return Ok(ratesLoaded);
        }
    }
}
