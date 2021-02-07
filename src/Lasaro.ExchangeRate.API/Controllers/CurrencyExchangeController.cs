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

            if (currencyExchangeModel.Direction == CurrencyExchangeDirection.SellForeign)
                return BadRequest("Selling foreign currency not fully implemented yet (missing limit business logic).");

            DateTime transactionDate = DateTime.Now;
            CurrencyQuoteModel foreignCurrencyRate = await RatesService.GetRateQuoteAsync(currencyExchangeModel.ForeignCurrencyCode,
                                                                                          transactionDate);

            if (foreignCurrencyRate == null)
                return BadRequest($"No rate found for {currencyExchangeModel.ForeignCurrencyCode} on {transactionDate}");

            double foreignCurrencyAmount = CurrencyExchangeService.CalculateForeignCurrencyAmount(currencyExchangeModel.Direction,
                                                                                                  currencyExchangeModel.LocalCurrencyAmount,
                                                                                                  foreignCurrencyRate.SellValue,
                                                                                                  foreignCurrencyRate.BuyValue);

            double userRemainingLimit = await CurrencyExchangeService.GetUserRemainingLimitAsync(currencyExchangeModel.UserId,
                                                                                                 currencyExchangeModel.ForeignCurrencyCode,
                                                                                                 transactionDate);

            //TODO: This validation works corretly when buying foreign, but needs refactor for selling
            if (userRemainingLimit < foreignCurrencyAmount)
                return BadRequest("Operation would exceed monthly limits. Operation cancelled.");

            CurrencyExchangeTransactionModel newTransaction = await CurrencyExchangeService.SubmitTransactionAsync(currencyExchangeModel,
                                                                                                                   foreignCurrencyAmount,
                                                                                                                   transactionDate,
                                                                                                                   foreignCurrencyRate.RateId);

            return Ok(newTransaction);
        }
    }
}
