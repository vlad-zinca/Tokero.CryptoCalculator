using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;

using Tokero.CryptoCalculator.Data.Repo;

namespace Tokero.CryptoCalculator.Business.Services;

public class PriceService : IPriceService
{
    private readonly Random _random = new();
    private readonly DataContext _context;
    

    public PriceService(DataContext context)
    {
        _context = context;
    }

    public decimal GetPrice(int cryptoId, DateTime date)
    {
        // Get the currency from the database
        var currency = _context.Currencies.SingleOrDefault(x => x.CurrencyId == cryptoId);
        if (currency == null)
        {
            // Log/throw an error
            return 0;
        }

        // Determine a fluctuation factor based on the currency type and the day
        var fluctuationRange = currency.Name == "Bitcoin" ? 0.05m : 0.15m; // 5% for Bitcoin, 15% for others

        // Apply the fluctuation to the price
        var priceForDay = ApplyFluctuation(currency.Price, fluctuationRange);

        return priceForDay;
    }

    private decimal ApplyFluctuation(decimal currentPrice, decimal fluctuationRange)
    {
        // Generate a random percentage within the range
        var randomPercentage = (decimal)_random.NextDouble() * fluctuationRange;

        // Randomly decide whether to increase or decrease
        bool increase = _random.Next(0, 2) == 0;

        // Apply the fluctuation
        var fluctuationAmount = currentPrice * randomPercentage;
        return increase ? currentPrice + fluctuationAmount : currentPrice - fluctuationAmount;
    }
}