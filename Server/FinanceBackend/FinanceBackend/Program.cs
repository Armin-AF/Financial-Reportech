using FinanceBackend.Services;
using Microsoft.Extensions.DependencyInjection;

// ...

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add YH Finance API and ChatGPT API clients as services
builder.Services.AddSingleton(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    return new YHFinanceApiClient(config["AppSettings:YHFinanceApiKey"], config["AppSettings:YHFinanceApiHost"]);
});
builder.Services.AddSingleton(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    return new ChatGPTApiClient(config["AppSettings:ChatGPTApiKey"], config["AppSettings:ChatGPTApiHost"]);
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