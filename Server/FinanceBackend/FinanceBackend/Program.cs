using FinanceBackend.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.AddConsole();

// Add YH Finance API and ChatGPT API clients as services
builder.Services.AddSingleton(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    return new YHFinanceApiClient(config["AppSettings:YHFinanceApiKey"], config["AppSettings:YHFinanceApiHost"]);
});

builder.Services.AddSingleton<ChatGPTApiClient>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var chatGPTApiKey = configuration["ChatGPTApiKey"];
    var chatGPTApiHost = configuration["ChatGPTApiHost"];
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>(); // Use IHttpClientFactory to create an HttpClient instance
    var logger = sp.GetRequiredService<ILogger<ChatGPTApiClient>>();
    return new ChatGPTApiClient(chatGPTApiKey, chatGPTApiHost, httpClientFactory.CreateClient(), logger);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();