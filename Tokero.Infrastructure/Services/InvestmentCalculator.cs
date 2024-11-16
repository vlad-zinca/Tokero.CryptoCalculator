using Tokero.CryptoCalculator.Data.Models;
using Tokero.CryptoCalculator.Data.Repo;

namespace Tokero.CryptoCalculator.Business.Services;

public class InvestmentCalculator : IInvestmentCalculator
{
    private readonly DataContext _context;
    private readonly IPriceService _priceService;

    public InvestmentCalculator(DataContext context, IPriceService priceService)
    {
        _context = context;
        _priceService = priceService;
    }

    public List<InvestmentResultGrouped> CalculateInvestmentAsync(Dictionary<int, decimal> currencyAmounts, Dictionary<int, DateTime?> currencyDates, Dictionary<int, string> currencyNames)
    {
        var results = new List<InvestmentResultGrouped>();

        foreach (var date in currencyDates)
        {
            var investmentPerCurrency = new List<InvestmentResult>();

            decimal totalInvested = 0;
            decimal totalCryptocurrency = 0;

            var currentDate = date.Value ?? DateTime.Now;
            while (currentDate <= DateTime.Today)
            {
                var currencyId = date.Key;
                var monthlyInvestment = currencyAmounts[date.Key];

                // Get the price for the cryptocurrency on the current date --> this will be mocked as Coin market does not have Pricing data for Basic plan
                decimal priceToday = _priceService.GetPriceAsync(currencyId, currentDate);

                // Calculate the amount of cryptocurrency bought
                decimal cryptocurrencyAmount = monthlyInvestment / priceToday;

                // Calculate the total invested
                totalInvested += monthlyInvestment;
                totalCryptocurrency += cryptocurrencyAmount;

                // Calculate the current value of the cryptocurrency
                decimal currentValue = totalCryptocurrency * priceToday;

                // Calculate ROI
                decimal roi = (currentValue - totalInvested) / totalInvested * 100;

                // Create the investment result for this month
                investmentPerCurrency.Add(new InvestmentResult
                {
                    Date = currentDate,
                    InvestedAmount = monthlyInvestment,
                    CryptocurrencyAmount = totalCryptocurrency,
                    ValueToday = currentValue,
                    ROI = roi
                });

                // Move to the next month
                currentDate = currentDate.AddMonths(1);
            }

            results.Add(new InvestmentResultGrouped()
            {
                CurrencyName = currencyNames[date.Key],
                Investments = investmentPerCurrency
            });
        }

        

        return results;
    }
}
