using FinanceBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SummariesController : ControllerBase
{
     readonly YHFinanceApiClient _yhFinanceApiClient;
     readonly ChatGPTApiClient _chatGPTApiClient;

    public SummariesController(IConfiguration configuration)
    {
        var yhFinanceApiKey = configuration["AppSettings:YHFinanceApiKey"];
        var yhFinanceApiHost = configuration["AppSettings:YHFinanceApiHost"];
        var chatGPTApiKey = configuration["AppSettings:ChatGPTApiKey"];
        var chatGPTApiHost = configuration["AppSettings:ChatGPTApiHost"];

        _yhFinanceApiClient = new YHFinanceApiClient(yhFinanceApiKey, yhFinanceApiHost);
        _chatGPTApiClient = new ChatGPTApiClient(chatGPTApiKey, chatGPTApiHost);
    }

    [HttpGet("generate-summary/{symbol}")]
    public async Task<ActionResult<string>> GenerateSummary(string symbol)
    {
        // Fetch the financial data for the given symbol
        var symbols = new List<string> { symbol };
        var companies = await _yhFinanceApiClient.FetchFinancialDataForSymbolsAsync(symbols);

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
