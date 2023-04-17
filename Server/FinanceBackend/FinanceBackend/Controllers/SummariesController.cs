using FinanceBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SummariesController : ControllerBase
{
     readonly ILogger<SummariesController> _logger;
     readonly YHFinanceApiClient _yhFinanceApiClient;
     readonly ChatGPTApiClient _chatGPTApiClient;


     public SummariesController(YHFinanceApiClient yhFinanceApiClient, ChatGPTApiClient chatGPTApiClient, ILogger<SummariesController> logger)
     {
         _yhFinanceApiClient = yhFinanceApiClient;
         _chatGPTApiClient = chatGPTApiClient;
         _logger = logger;
     }


     [HttpGet("generate-summary/{symbol}")]
     public async Task<ActionResult<string>> GenerateSummary(string symbol)
     {
         try
         {
             _logger.LogInformation($"Fetching financial data for symbol: {symbol}");
             var symbols = new List<string> { symbol };
             var companies = await _yhFinanceApiClient.FetchFinancialDataForSymbolsAsync(symbols);

             if (companies.Count == 0)
             {
                 _logger.LogWarning($"Financial data for symbol '{symbol}' not found.");
                 return NotFound($"Financial data for symbol '{symbol}' not found.");
             }

             // Generate the summary using the ChatGPT API client
             var company = companies[0];
             _logger.LogInformation($"Generating summary for symbol: {symbol}");
             string summary = await _chatGPTApiClient.GenerateSummaryAsync(company);

             return Ok(summary);
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, $"Error generating summary for symbol: {symbol}");
             return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while generating the summary.");
         }
     }
}
