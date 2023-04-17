namespace FinanceBackend.Models;

public class HistoricalData
{
    public DateTime Date { get; set; }
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public decimal AdjClose { get; set; }
    public long Volume { get; set; }
    public string Symbol { get; set; }
}