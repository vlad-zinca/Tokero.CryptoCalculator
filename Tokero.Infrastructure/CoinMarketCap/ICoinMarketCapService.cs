using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tokero.CryptoCalculator.Business.CoinMarketCap
{
    public interface ICoinMarketCapService
    {
        Task<List<Data.Models.Currency>> GetCurrencies();
    }
}
