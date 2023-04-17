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

    public ChatGPTApiClient(string apiKey, string apiHost)
    {
        _apiKey = apiKey;
        _apiHost = apiHost;
    }

    public async Task<string> GenerateSummaryAsync(Company company)
    {
        var httpClient = new HttpClient();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(BaseUrl),
            Headers =
            {
                { "X-RapidAPI-Key", _apiKey },
                { "X-RapidAPI-Host", _apiHost },
            },
            Content = new StringContent(JsonSerializer.Serialize(new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = $"Summarize the financial performance of {company.Name} with the symbol {company.Symbol}."
                    }
                }
            }), Encoding.UTF8, "application/json")
        };

        using var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<ChatGPTApiResponse>(jsonResponse);

        return responseObject?.Choices[0].Text?.Trim() ?? "Failed to generate a summary.";
    }
}