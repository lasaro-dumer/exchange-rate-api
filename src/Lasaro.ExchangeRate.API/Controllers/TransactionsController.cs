using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lasaro.ExchangeRate.API.Models;
using Lasaro.ExchangeRate.API.Services.Abstractions;
using Lasaro.ExchangeRate.Data.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Lasaro.ExchangeRate.API.Controllers
{
    [ApiController]
    [Route("transactions")]
    public class TransactionsController : Controller
    {
        public ICurrencyExchangeService CurrencyExchangeService { get; }

        public TransactionsController(ICurrencyExchangeService currencyExchangeService)
        {
            CurrencyExchangeService = currencyExchangeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactionsAsync()
        {
            List<CurrencyExchangeTransactionModel> currencyTransactionModels = await CurrencyExchangeService.GetTransactionsAsync();

            return Ok(currencyTransactionModels);
        }
    }
}
