using System.Text.Json;
using Flurl;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl.Http;
using FinanceBackend.Models;

namespace FinanceBackend.Services;


public class YahooFinanceApiClient
{
    private const string BaseUrl = "https://yahoo-finance127.p.rapidapi.com/finance-analytics/";
    private readonly string _apiKey;
    private readonly string _apiHost;

    public YahooFinanceApiClient(string apiKey, string apiHost)
    {
        _apiKey = apiKey;
        _apiHost = apiHost;
    }

    public async Task<List<Company>> FetchFinancialDataForSymbolsAsync(IEnumerable<string> symbols)
    {
        var httpClient = new HttpClient();
        var companies = new List<Company>();

        foreach (var symbol in symbols)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(BaseUrl + symbol),
                Headers =
                {
                    { "X-RapidAPI-Key", _apiKey },
                    { "X-RapidAPI-Host", _apiHost },
                },
            };

            using var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var company = JsonSerializer.Deserialize<Company>(jsonResponse);

            if (company != null)
            {
                companies.Add(company);
            }
        }

        return companies;
    }
}