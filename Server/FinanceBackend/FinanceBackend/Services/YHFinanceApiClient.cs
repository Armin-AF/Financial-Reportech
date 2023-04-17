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

    public async Task<List<Company>> FetchFinancialDataForSymbolsAsync(IEnumerable<string> symbols)
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", _apiKey);
        httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", _apiHost);

        var companies = new List<Company>();

        foreach (var symbol in symbols)
        {
            var response = await httpClient.GetAsync($"https://yh-finance-complete.p.rapidapi.com/yhf?ticker={symbol}");

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var financialData = JsonConvert.DeserializeObject<Company>(responseBody);
                companies.Add(financialData);
            }
        }

        return companies;
    }
}