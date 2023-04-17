using FinanceBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SummariesController : ControllerBase
{
     readonly YahooFinanceApiClient _yahooFinanceApiClient;
     readonly ChatGPTApiClient _chatGPTApiClient;

    public SummariesController(IConfiguration configuration)
    {
        var yahooFinanceApiKey = configuration["AppSettings:YahooFinanceApiKey"];
        var yahooFinanceApiHost = configuration["AppSettings:YahooFinanceApiHost"];
        var chatGPTApiKey = configuration["AppSettings:OpenAIApiKey"];

        _yahooFinanceApiClient = new YahooFinanceApiClient(yahooFinanceApiKey, yahooFinanceApiHost);
        _chatGPTApiClient = new ChatGPTApiClient(chatGPTApiKey);
    }

    [HttpGet("generate-summary/{symbol}")]
    public async Task<ActionResult<string>> GenerateSummary(string symbol)
    {
        // Fetch the financial data for the given symbol
        var symbols = new List<string> { symbol };
        var companies = await _yahooFinanceApiClient.FetchFinancialDataForSymbolsAsync(symbols);

        if (companies.Count == 0)
        {
            return NotFound($"Financial data for symbol '{symbol}' not found.");
        }

        // Generate the summary using the ChatGPT API client
        var company = companies[0];
        string summary = await _chatGPTApiClient.GenerateSummaryAsync(company);

        return Ok(summary);
    }
}
