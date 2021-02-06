using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lasaro.ExchangeRate.API.Models;
using Lasaro.ExchangeRate.API.Services.Abstractions;
using Lasaro.ExchangeRate.Data.Entities;
using Lasaro.ExchangeRate.Data.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Lasaro.ExchangeRate.API.Controllers
{
    [ApiController]
    public class SeedController : Controller
    {
        public IRatesRepository RatesRepository { get; }

        public SeedController(IRatesRepository ratesRepository)
        {
            RatesRepository = ratesRepository;
        }

        [HttpGet("/seed")]
        public async Task<IActionResult> SeedAsync()
        {
            bool ratesExist = (await RatesRepository.GetRatesAsync()).Any();

            if (!ratesExist)
            {
                Rate usdRate = new Rate()
                {
                    CurrencyCode = "USD",
                    BuyValue = 86.750,
                    SellValue = 92.750,
                    EffectiveDate = new DateTime(2021, 2, 4, 15, 0, 0)
                };

                Rate brlRate = new Rate()
                {
                    CurrencyCode = "BRL",
                    BuyValue = usdRate.BuyValue / 4,
                    SellValue = usdRate.SellValue / 4,
                    EffectiveDate = new DateTime(2021, 2, 4, 15, 0, 0)
                };

                RatesRepository.AddRate(usdRate);
                RatesRepository.AddRate(brlRate);

                await RatesRepository.SaveChangesAsync();

                return Ok("Database seeded");
            }

            return Ok("Database was already seeded");
        }
    }
}
