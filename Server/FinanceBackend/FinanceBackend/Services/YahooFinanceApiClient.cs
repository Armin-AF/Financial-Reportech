using Flurl;

namespace FinanceBackend.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl.Http;
using FinanceBackend.Models;

public class YahooFinanceApiClient
{
    const string BaseUrl = "https://query1.finance.yahoo.com/v7/finance/quote";

    readonly string _apiKey;

    public YahooFinanceApiClient(string apiKey)
    {
        _apiKey = apiKey;
    }

    public async Task<List<Company>> FetchFinancialDataForSymbolsAsync(List<string> symbols)
    {
        var response = await BaseUrl
            .SetQueryParam("symbols", string.Join(",", symbols))
            .SetQueryParam("apiKey", _apiKey)
            .GetJsonAsync<YahooFinanceApiResponse>();

        var companies = new List<Company>();

        foreach (var result in response.QuoteResponse.Result)
        {
            var company = new Company
            {
                Name = result.LongName,
                Symbol = result.Symbol,
                Date = DateTime.UtcNow.Date,
                Open = result.RegularMarketOpen,
                High = result.RegularMarketDayHigh,
                Low = result.RegularMarketDayLow,
                Close = result.RegularMarketPreviousClose,
                Volume = result.RegularMarketVolume
            };

            companies.Add(company);
        }

        return companies;
    }
}