using System.Collections.Generic;

namespace Lasaro.ExchangeRate.API.Models
{
    public class LoaderResultModel
    {
        public List<CurrencyQuoteModel> RatesLoaded { get; set; }
        public List<string> Errors { get; set; }
        public LoaderResultModel()
        {
            RatesLoaded = new List<CurrencyQuoteModel>();
            Errors = new List<string>();
        }
    }
}
