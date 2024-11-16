using Microsoft.EntityFrameworkCore;

using System.Text.Json;
using Tokero.CryptoCalculator.Data.Repo;

namespace Tokero.CryptoCalculator.Business.CoinMarketCap;

public class CoinMarketCapService : ICoinMarketCapService
{
    private static string API_KEY = "86a604b2-cb72-4d7e-9574-60360b35a886";

    private readonly DataContext _dbContext;
    private readonly IHttpClientFactory _httpClientFactory;

    public CoinMarketCapService(DataContext dbContext, IHttpClientFactory httpClientFactory)
    {
        _dbContext = dbContext;
        _httpClientFactory = httpClientFactory;
    }

    private const int CacheDurationDays = 7; // Cache data for 7 days + store it in DB so I do not make useless call to coinmarket

    public async Task<List<Data.Models.Currency>> GetCurrencies()
    {
        // Check if cached data is available and recent enough
        var lastUpdatedCurrency = _dbContext.Currencies.OrderByDescending(c => c.LastUpdated).FirstOrDefault();
        if (lastUpdatedCurrency != null && (DateTime.UtcNow - lastUpdatedCurrency.LastUpdated).Days < CacheDurationDays)
        {
            return await _dbContext.Currencies.OrderBy(x => x.Rank).ToListAsync();
        }

        // Fetch data from the API if cache is stale or empty
        var result = await FetchCurrenciesFromApi();
        if (result == null || !result.Data.Any())
        {
            //TODO Fix error msg
            throw new Exception("Some issue");
        }

        // Clear out the old data and store the new data in the database
        _dbContext.Currencies.RemoveRange(_dbContext.Currencies);
        var dbList = result.Data.Select(currency => new Data.Models.Currency()
            {
                CurrencyId = currency.Id,
                Name = currency.Name,
                DateAdded = currency.DateAdded,
                LastUpdated = currency.LastUpdated,
                NumMarketPairs = currency.NumMarketPairs,
                Rank = currency.Rank,
                Slug = currency.Slug,
                Symbol = currency.Symbol,
                Price = currency.Quote?.Usd?.Price ?? 0,
            })
            .ToList().OrderBy(x => x.Rank);

        await _dbContext.AddRangeAsync(dbList);
        await _dbContext.SaveChangesAsync();

        return [];
    }

    private async Task<GetCurrenciesResponse> FetchCurrenciesFromApi()
    {
        var baseUrl = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest";
        var queryString = $"start={Uri.EscapeDataString("1")}&limit={Uri.EscapeDataString("5000")}&convert={Uri.EscapeDataString("USD")}";
        var fullUrl = $"{baseUrl}?{queryString}";

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", API_KEY);
        client.DefaultRequestHeaders.Add("Accept", "application/json");

        try
        {
            var response = await client.GetAsync(fullUrl);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<GetCurrenciesResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Update last updated timestamp
                var currencies = apiResponse?.Data;
                currencies?.ForEach(currency => currency.LastUpdated = DateTime.UtcNow);

                return apiResponse ?? new GetCurrenciesResponse();
            }

            Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            return new GetCurrenciesResponse();
        }
        catch (HttpRequestException httpRequestEx)
        {
            Console.WriteLine($"Request error: {httpRequestEx.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            throw;
        }
    }
}