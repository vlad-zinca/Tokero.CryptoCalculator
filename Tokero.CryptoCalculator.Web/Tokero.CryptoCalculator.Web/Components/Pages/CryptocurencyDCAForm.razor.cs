using Microsoft.AspNetCore.Components;

using MudBlazor;
using Tokero.CryptoCalculator.Business.Services;
using Currency = Tokero.CryptoCalculator.Data.Models.Currency;

namespace Tokero.CryptoCalculator.Web.Components.Pages;

public partial class CryptocurencyDCAForm: ComponentBase
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = null!;

    [Parameter] public HashSet<Currency> Currencies { get; set; } = new();
    [Parameter] public decimal AvailableInvestmentAmount { get; set; }

    [Inject]
    public IInvestmentCalculator InvestmentCalculator { get; set; } = default!;

    private Dictionary<int, decimal> CurrencyAmounts { get; set; } = new();
    private Dictionary<int, DateTime?> CurrencyDates { get; set; } = new();
    private Dictionary<int, string> CurrencyNames { get; set; } = new();

    private Dictionary<int, string> CurrencyErrors = new();

    private decimal RemainingInvestmentAmount
    {
        get => AvailableInvestmentAmount - CurrencyAmounts.Values.Sum();
    }

    protected override void OnParametersSet()
    {
        if (!Currencies.Any())
        {
            return;
        }

        CurrencyAmounts.Clear();
        CurrencyDates.Clear();
        CurrencyNames.Clear();

        foreach (var currency in Currencies)
        {
            CurrencyAmounts.Add(currency.CurrencyId, 0);
            CurrencyDates.Add(currency.CurrencyId, DateTime.Now);
            CurrencyNames.Add(currency.CurrencyId, currency.Name);
        }
    }

    private decimal GetCurrencyAmount(int currencyId) =>
        CurrencyAmounts.GetValueOrDefault(currencyId, 0);

    private void SetCurrencyAmount(int currencyId, decimal amount)
    {
        if (amount >= 0 && amount <= RemainingInvestmentAmount + GetCurrencyAmount(currencyId))
        {
            CurrencyAmounts[currencyId] = amount;
            CurrencyErrors.Remove(currencyId);
        }
        else
        {
            // Add error if amount exceeds remaining balance or is negative
            CurrencyErrors[currencyId] = "Amount exceeds remaining balance or is invalid.";
        }
    }

    private DateTime? GetCurrencyDate(int currencyId) =>
        CurrencyDates.GetValueOrDefault(currencyId);

    private void SetCurrencyDate(int currencyId, DateTime? date)
    {
        if (date.HasValue)
        {
            CurrencyDates[currencyId] = date;
        }
    }

    private string GetErrorText(int currencyId) =>
        CurrencyErrors.ContainsKey(currencyId) ? CurrencyErrors[currencyId] : string.Empty;

    private void Submit()
    {
        var result = InvestmentCalculator.CalculateInvestmentAsync(CurrencyAmounts, CurrencyDates, CurrencyNames);
        MudDialog.Close(DialogResult.Ok(result));
    }


    private void Cancel() => MudDialog.Cancel();
}