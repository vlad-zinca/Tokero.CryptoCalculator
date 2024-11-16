using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Services;

using Tokero.CryptoCalculator.Business.CoinMarketCap;
using Tokero.CryptoCalculator.Business.Services;
using Tokero.CryptoCalculator.Data.Repo;
using Tokero.CryptoCalculator.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register IHttpClientFactory for managing HttpClient instances
builder.Services.AddHttpClient();

builder.Services.AddMudServices();
// Register services
builder.Services.AddMudServices();
builder.Services.AddScoped<IPriceService, PriceService>();
builder.Services.AddScoped<IInvestmentCalculator, InvestmentCalculator>();
builder.Services.AddScoped<ICoinMarketCapService, CoinMarketCapService>();
builder.Services.AddScoped<IDialogService, DialogService>();

// Register the database context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();