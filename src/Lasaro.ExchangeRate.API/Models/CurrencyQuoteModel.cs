﻿using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lasaro.ExchangeRate.API.Models
{
    public class CurrencyQuoteModel
    {
        public int RateId { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime EffectiveDate { get; set; }
        public double BuyValue { get; set; }
        public double SellValue { get; set; }
    }
}
