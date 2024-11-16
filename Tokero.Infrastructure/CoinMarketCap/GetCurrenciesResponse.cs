using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Tokero.CryptoCalculator.Business.CoinMarketCap;

public class GetCurrenciesResponse
{
    //Maybe add status
    public List<Currency> Data { get; set; }
}

public class Currency
{
    public int Id { get; set; }
    public int CurrencyId { get; set; }
    public string Name { get; set; }
    public string Symbol { get; set; }
    public string Slug { get; set; }
    [JsonPropertyName("num_market_pairs")]
    public int NumMarketPairs { get; set; }

    [JsonPropertyName("date_added")]
    public DateTime DateAdded { get; set; }
    [JsonPropertyName("cmc_rank")]
    public int Rank { get; set; }
    [JsonPropertyName("last_updated")]
    public DateTime LastUpdated { get; set; }

    [JsonPropertyName("quote")]
    public Quote? Quote { get; set; }
}

public class Quote
{
    [JsonPropertyName("USD")]
    public UsdQuote? Usd { get; set; }
}

public class UsdQuote
{
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
}