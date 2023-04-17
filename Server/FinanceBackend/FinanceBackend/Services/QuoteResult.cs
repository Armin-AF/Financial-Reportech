namespace FinanceBackend.Services;

public class QuoteResult
{
    public string Symbol { get; set; }
    public string LongName { get; set; }
    public decimal RegularMarketOpen { get; set; }
    public decimal RegularMarketDayHigh { get; set; }
    public decimal RegularMarketDayLow { get; set; }
    public decimal RegularMarketPreviousClose { get; set; }
    public long RegularMarketVolume { get; set; }
}