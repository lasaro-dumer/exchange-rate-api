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
        public IEnumerable<ICurrencyRateLoaderService> CurrencyRateLoaders { get; }

        public QuotesController(IRatesService ratesService,
                                IEnumerable<ICurrencyRateLoaderService> currencyRateLoaders)
        {
            RatesService = ratesService;
            CurrencyRateLoaders = currencyRateLoaders.OrderBy(cl => cl.Priority);
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
            LoaderResultModel loaderResultModel = new LoaderResultModel();

            foreach (ICurrencyRateLoaderService currencyLoader in CurrencyRateLoaders)
            {
                try
                {
                    loaderResultModel.RatesLoaded.Add(await currencyLoader.LoadRateAsync());
                }
                catch (Exception ex)
                {
                    loaderResultModel.Errors.Add($"Error loading rate for currency {currencyLoader.CurrencyCode}.");
                }
            }

            return Ok(loaderResultModel);
        }
    }
}
