namespace Tokero.CryptoCalculator.Business.Services;

public interface IPriceService
{
    decimal GetPriceAsync(int cryptoId, DateTime date);
}
