using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lasaro.ExchangeRate.API.Models;
using Lasaro.ExchangeRate.API.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Lasaro.ExchangeRate.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuotesController : ControllerBase
    {
        public IRatesService RatesService { get; }

        public QuotesController(IRatesService ratesService)
        {
            RatesService = ratesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllQuotesAsync(DateTime? date = null)
        {
            return Ok(await RatesService.GetRatesAsync(date: date));
        }

        [HttpGet("{currencyCode}")]
        public async Task<IActionResult> GetAsync(string currencyCode, DateTime? date = null)
        {
            DateTime effectiveDate = DateTime.Now.Date;

            if (date.HasValue)
                effectiveDate = date.Value.Date;

            CurrencyQuoteModel currencyQuote = await RatesService.GetRateQuoteAsync(currencyCode, effectiveDate);

            return Ok(currencyQuote);
        }
    }
}
