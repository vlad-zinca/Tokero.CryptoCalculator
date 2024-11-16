using System;
using System.Collections.Generic;
using Tokero.CryptoCalculator.Data.Models;

namespace Tokero.CryptoCalculator.Business.Services;

public interface IInvestmentCalculator
{
    List<InvestmentResultGrouped> CalculateInvestmentAsync(Dictionary<int, decimal> currencyAmounts,
        Dictionary<int, DateTime?> currencyDates, Dictionary<int, string> currencyNames);
}