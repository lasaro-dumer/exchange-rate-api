namespace Lasaro.ExchangeRate.API.Models
{
    public class CurrencyExchangeModel
    {
        public int UserId { get; set; }
        public string ForeignCurrencyCode { get; internal set; }
        public CurrencyExchangeDirection Direction { get; internal set; }
        public double LocalCurrencyAmount { get; internal set; }
    }
}
