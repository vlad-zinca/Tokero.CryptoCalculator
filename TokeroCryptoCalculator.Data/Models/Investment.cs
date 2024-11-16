using System;

namespace Tokero.CryptoCalculator.Data.Models;

public class Investment
{
    public int Id { get; set; }
    public int CryptoId { get; set; }
    public DateTime StartDate { get; set; }
    public decimal MonthlyAmount { get; set; }
    public int InvestmentDay { get; set; }
}