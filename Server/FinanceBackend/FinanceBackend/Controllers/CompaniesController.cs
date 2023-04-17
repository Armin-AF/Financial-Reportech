using FinanceBackend.Models;
using FinanceBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly YahooFinanceApiClient _yahooFinanceApiClient;

    public CompaniesController()
    {
        var apiKey = "your_yahoo_finance_api_key";
        _yahooFinanceApiClient = new YahooFinanceApiClient(apiKey);
    }

    [HttpGet("financial-data")]
    public async Task<ActionResult<List<Company>>> GetFinancialData()
    {
        var symbols = new List<string>
        {
            "AAPL", "MSFT", "GOOG", "AMZN", "FB", "TSLA", "NVDA", "ADBE", "ASML", "INTC"
        };

        var companies = await _yahooFinanceApiClient.FetchFinancialDataForSymbolsAsync(symbols);
        return Ok(companies);
    }
}