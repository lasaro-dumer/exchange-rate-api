using System.Collections.Generic;

namespace Lasaro.ExchangeRate.API.Models
{
    public class RefreshResultModel
    {
        public List<CurrencyQuoteModel> RatesLoaded { get; set; }
        public List<string> Errors { get; set; }
        public RefreshResultModel()
        {
            RatesLoaded = new List<CurrencyQuoteModel>();
            Errors = new List<string>();
        }
    }
}
