using System;

namespace Tokero.CryptoCalculator.Business.Services;

public interface IPriceService
{
    decimal GetPrice(int cryptoId, DateTime date);
}
