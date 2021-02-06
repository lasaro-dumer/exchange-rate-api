using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lasaro.ExchangeRate.API.Models;
using Lasaro.ExchangeRate.API.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Lasaro.ExchangeRate.API.Controllers
{
    [ApiController]
    [Route("currency/exchange")]
    public class CurrencyExchangeController : Controller
    {
        public IRatesService RatesService { get; }
        public ICurrencyExchangeService CurrencyExchangeService { get; }

        public CurrencyExchangeController(IRatesService ratesService, ICurrencyExchangeService currencyExchangeService)
        {
            RatesService = ratesService;
            CurrencyExchangeService = currencyExchangeService;
        }

        [HttpPost("transaction")]
        public async Task<IActionResult> ExecuteCurrencyExchangeAsync(CurrencyExchangeModel currencyExchangeModel)
        {
            if (!await RatesService.IsValidCurrencyToQuoteAsync(currencyExchangeModel.ForeignCurrencyCode))
                return BadRequest("Foreign currency not valid");

            DateTime transactionDate = DateTime.Now;
            CurrencyQuoteModel ForeignCurrencyRate = await RatesService.GetRateQuoteAsync(currencyExchangeModel.ForeignCurrencyCode,
                                                                                          transactionDate);

            if (ForeignCurrencyRate == null)
                return BadRequest($"No rate found for {currencyExchangeModel.ForeignCurrencyCode} on {transactionDate}");

            double ForeignCurrencyAmount = CurrencyExchangeService.CalculateForeignCurrencyAmount(currencyExchangeModel.Direction,
                                                                                                  currencyExchangeModel.LocalCurrencyAmount,
                                                                                                  ForeignCurrencyRate.SellValue,
                                                                                                  ForeignCurrencyRate.BuyValue);

            double userRemainingLimit = await CurrencyExchangeService.GetUserRemainingLimitAsync(currencyExchangeModel.UserId,
                                                                                                 currencyExchangeModel.ForeignCurrencyCode,
                                                                                                 transactionDate);

            if (userRemainingLimit < ForeignCurrencyAmount)
                return BadRequest("Operation would exceed monthly limits. Operation cancelled.");

            return Ok("Operation successful");
        }
    }
}
