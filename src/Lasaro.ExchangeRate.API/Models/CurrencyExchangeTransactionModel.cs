using System;

namespace Lasaro.ExchangeRate.API.Models
{
    public class CurrencyExchangeTransactionModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double LocalCurrencyAmount { get; set; }
        public double ForeignCurrencyAmount { get; set; }
        public string ForeignCurrencyCode { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
