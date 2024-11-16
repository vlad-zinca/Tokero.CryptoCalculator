namespace Tokero.CryptoCalculator.Data.Models;

public class PriceHistory
{
    public int Id { get; set; }
    public int CryptoId { get; set; }
    public DateTime Date { get; set; }
    public decimal Price { get; set; }
}