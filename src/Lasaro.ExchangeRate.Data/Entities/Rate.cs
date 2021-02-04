using System;
using System.Collections.Generic;
using System.Text;

namespace Lasaro.ExchangeRate.Data.Entities
{
    public class Rate
    {
        public int Id { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime EffectiveDate { get; set; }
        public double BuyValue { get; set; }
        public double SellValue { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
