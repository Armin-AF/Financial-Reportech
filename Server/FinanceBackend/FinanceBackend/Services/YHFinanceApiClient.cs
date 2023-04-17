using FinanceBackend.Models;
using Newtonsoft.Json;

namespace FinanceBackend.Services;

public class YHFinanceApiClient
{
    private readonly string _apiKey;
    private readonly string _apiHost;

    public YHFinanceApiClient(string apiKey, string apiHost)
    {
        _apiKey = apiKey;
        _apiHost = apiHost;
    }

    public async Task<List<Company>> FetchFinancialDataForSymbolsAsync(IEnumerable<string> symbols, DateTime startDate, DateTime endDate)
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", _apiKey);
        httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", _apiHost);

        var companies = new List<Company>();

        foreach (var symbol in symbols)
        {
            var url = $"https://yh-finance-complete.p.rapidapi.com/yhfhistorical?ticker={symbol}&sdate={startDate.ToString("yyyy-MM-dd")}&edate={endDate.ToString("yyyy-MM-dd")}";
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var historicalData = JsonConvert.DeserializeObject<List<HistoricalData>>(responseBody);

                // Convert HistoricalData objects into HistoricalDatum objects
                var historicalDataList = historicalData.Select(h => new HistoricalDatum {
                    Date = h.Date,
                    Open = h.Open,
                    High = h.High,
                    Low = h.Low,
                    Close = h.Close,
                    Volume = h.Volume,
                    AdjClose = h.AdjClose
                }).ToList();

                var company = new Company { Symbol = symbol, HistoricalData = historicalDataList };
                companies.Add(company);
            }
        }

        return companies;
    }

}