using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lasaro.ExchangeRate.Data.Entities
{
    public class CurrencyExchangeTransaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double LocalCurrencyAmount { get; set; }
        public string ForeignCurrencyCode { get; set; }
        public double ForeignCurrencyAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public virtual Rate Rate { get; set; }
        public int RateId { get; set; }
    }
}
