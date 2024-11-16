namespace Tokero.CryptoCalculator.Data.Models;

public class InvestmentResult
{
    public int CryptoCurrencyId {get; set; }
    public string CryptoCurrencyName {get; set; }
    public DateTime Date { get; set; }
    public decimal InvestedAmount { get; set; }
    public decimal CryptocurrencyAmount { get; set; }
    public decimal ValueToday { get; set; }
    public decimal ROI { get; set; }
    public decimal TotalValueToday { get; set; }
}

public class InvestmentResultGrouped
{
    public string CurrencyName { get; set; }
    public List<InvestmentResult> Investments { get; set; }
    public decimal CoinProfitTotal { get; set; }
}