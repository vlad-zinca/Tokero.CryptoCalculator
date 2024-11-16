namespace Tokero.CryptoCalculator.Business.CoinMarketCap
{
    public interface ICoinMarketCapService
    {
        Task<List<Data.Models.Currency>> GetCurrencies();
    }
}
