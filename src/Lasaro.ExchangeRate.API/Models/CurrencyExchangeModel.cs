namespace Lasaro.ExchangeRate.API.Models
{
    public class CurrencyExchangeModel
    {
        public int UserId { get; set; }
        public string ForeignCurrencyCode { get; set; }
        public CurrencyExchangeDirection Direction { get; set; }
        public double LocalCurrencyAmount { get; set; }
    }
}
