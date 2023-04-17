using FinanceBackend.Models;
using Flurl.Http;

namespace FinanceBackend.Services;

public class ChatGPTApiClient
{
    private const string BaseUrl = "https://api.openai.com/v1/engines/davinci-codex/completions";

    private readonly string _apiKey;

    public ChatGPTApiClient(string apiKey)
    {
        _apiKey = apiKey;
    }

    public async Task<string> GenerateSummaryAsync(Company company)
    {
        var prompt = $"Generate a brief summary of the financial performance for {company.Name} ({company.Symbol}) on {company.Date:MMMM dd, yyyy}. The company opened at {company.Open}, reached a high of {company.High}, a low of {company.Low}, and closed at {company.Close}. The trading volume was {company.Volume}.";

        var requestBody = new
        {
            prompt = prompt,
            max_tokens = 50,
            n = 1,
            stop = string.Empty,
            temperature = 0.7
        };

        var response = await BaseUrl
            .WithHeader("Authorization", $"Bearer {_apiKey}")
            .PostJsonAsync(requestBody)
            .ReceiveJson<ChatGPTApiResponse>();

        if (response?.Choices.Count > 0)
        {
            return response.Choices[0].Text.Trim();
        }

        return string.Empty;
    }
}