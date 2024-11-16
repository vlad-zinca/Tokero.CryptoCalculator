namespace Tokero.CryptoCalculator.Data.Models;

public class Currency
{
    public int Id { get; set; }
    public int CurrencyId { get; set; }
    public string Name { get; set; }
    public string Symbol { get; set; }
    public string Slug { get; set; }
    public int NumMarketPairs { get; set; }
    public DateTime DateAdded { get; set; }
    public int Rank { get; set; }
    public DateTime LastUpdated { get; set; }
    public decimal Price { get; set; }
}