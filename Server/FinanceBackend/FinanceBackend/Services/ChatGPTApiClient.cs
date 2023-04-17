using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FinanceBackend.Models;
using Flurl.Http;

namespace FinanceBackend.Services;

public class ChatGPTApiClient
{
    const string BaseUrl = "https://openai80.p.rapidapi.com/chat/completions";
    readonly string _apiKey;
    readonly string _apiHost;
    private readonly HttpClient _httpClient;
    private readonly ILogger<ChatGPTApiClient> _logger;

    public ChatGPTApiClient(string apiKey, string apiHost, HttpClient httpClient, ILogger<ChatGPTApiClient> logger){
        _apiKey = apiKey;
        _apiHost = apiHost;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<string> GenerateSummaryAsync(Company company){
        var request = new HttpRequestMessage{
            Method = HttpMethod.Post,
            RequestUri = new Uri(BaseUrl),
            Headers ={
                {"X-RapidAPI-Key", _apiKey},
                {"X-RapidAPI-Host", _apiHost},
            },
            Content = new StringContent(JsonSerializer.Serialize(new{
                model = "gpt-3.5-turbo-0301",
                messages = new[]{
                    new{
                        role = "user",
                        content =
                            $"Summarize the financial performance of {company.Name} with the symbol {company.Symbol}."
                    }
                }
            }), Encoding.UTF8, "application/json")
        };

        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        try{
            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"ChatGPT API response: {jsonResponse}");
            var responseObject = JsonSerializer.Deserialize<ChatGPTApiResponse>(jsonResponse);

            return responseObject?.Choices[0].Text?.Trim() ?? "Failed to generate a summary.";
        }
        catch (Exception ex){
            _logger.LogError(ex, "Error while generating summary with ChatGPT API");
            throw;
        }
    }
}