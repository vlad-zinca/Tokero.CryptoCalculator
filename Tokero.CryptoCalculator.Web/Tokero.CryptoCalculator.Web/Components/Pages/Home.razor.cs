using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;
using MudBlazor;

using Tokero.CryptoCalculator.Business.CoinMarketCap;
using Tokero.CryptoCalculator.Data.Models;
using Tokero.CryptoCalculator.Web.Components.Common;
using Currency = Tokero.CryptoCalculator.Data.Models.Currency;

namespace Tokero.CryptoCalculator.Web.Components.Pages
{
    public partial class Home: TokeroComponentBase
    {
        [Inject]
        private ICoinMarketCapService CustomerContext { get; set; } = default!;

        [Inject]
        private IDialogService DialogService { get; set; } = default!;

        [ObservableProperty] 
        private List<Data.Models.Currency> _currencies = new ();

        [ObservableProperty]
        private HashSet<Currency> _selectedCurrencies = new();

        [ObservableProperty]
        private List<InvestmentResultGrouped> _investmentResults = new ();

        [ObservableProperty] 
        private decimal _availableInvestmentAmount = 0;

        [ObservableProperty]
        private bool _updateInvestmentAmount = false;

        [ObservableProperty] private bool _hasError = false;

        [ObservableProperty]
        private string _errorText = string.Empty;

        [ObservableProperty]
        private TableGroupDefinition<InvestmentResult> _groupDefinition = new()
        {
            GroupName = "Group:",
            Indentation = false,
            Expandable = true,
            Selector = (e) => e.CryptoCurrencyName
        };

        [ObservableProperty]
        private List<InvestmentResult> _flattenedInvestmentResults = new();

        protected override async Task OnInitializedAsync()
        {
            var currencies = await CustomerContext.GetCurrencies();

            Currencies.Clear();
            Currencies.AddRange(currencies);
        }

        private async Task OnSearch(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                var currencies = await CustomerContext.GetCurrencies();
                Currencies.Clear();
                Currencies.AddRange(currencies);
                return;
            }

            var result = Currencies.Where(x => x.Name.StartsWith(text.Trim())).ToList();
            
            Currencies.Clear();
            Currencies.AddRange(result);
        }

        private async Task OpenCryptoDcaDialog()
        {
            if (!SelectedCurrencies.Any())
            {
                //TODO show some error that they need to select at least 1
                return;
            }

            var parameters = new DialogParameters
            {
                { "Currencies", SelectedCurrencies },
                { "AvailableInvestmentAmount", AvailableInvestmentAmount }
            };

            var dialogTitle = "Add investment strategy";
            var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraLarge, FullWidth = true};

            var dialogRef = await DialogService.ShowAsync<CryptocurencyDCAForm>(dialogTitle, parameters, options);
            var dialogResult = await dialogRef.Result;

            if (dialogResult is { Canceled: false, Data: List<InvestmentResultGrouped> })
            {
                var groupedInvestmentsCalculated = dialogResult!.Data as List<InvestmentResultGrouped>;

                if (groupedInvestmentsCalculated != null && groupedInvestmentsCalculated.Any())
                {
                    foreach (var group in groupedInvestmentsCalculated)
                    {
                        GroupDefinition.GroupName = $"Group: {group.CurrencyName}";

                        //Clear any old group data
                        FlattenedInvestmentResults.Clear();
                        InvestmentResults.RemoveAll(target => target.CurrencyName == group.CurrencyName);
                        InvestmentResults.AddRange(new List<InvestmentResultGrouped>()
                        {
                            new()
                            {
                                CurrencyName = group.CurrencyName,
                                Investments = group.Investments
                            }
                        });
                        
                        FlattenedInvestmentResults = InvestmentResults
                            .SelectMany(group => group.Investments.Select(investment =>
                            {
                                investment.CryptoCurrencyName = group.CurrencyName;
                                return investment;
                            }))
                            .ToList();

                        InvestmentResults.FirstOrDefault(x => x.CurrencyName == group.CurrencyName)!.CoinProfitTotal =group.Investments.Last().ValueToday;
                    }
                }
            }
        }

        partial void OnAvailableInvestmentAmountChanged(decimal value)
        {
            if (value <= 0)
            {
                HasError = true;
                ErrorText = "Investment amount must be greater than 0.";
            }
            else
            {
                HasError = false;
                ErrorText = string.Empty;
            }

            AvailableInvestmentAmount = value;
        }

        public bool Toggle(bool input)
        {
            UpdateInvestmentAmount = input;
            return !input;
        }

        private EventCallback<bool> GetValueChangedCallback()
        {
            return EventCallback.Factory.Create<bool>(this, isEnabled => Toggle(isEnabled));
        }

        private CurrencyComparer Comparer = new();

        private class CurrencyComparer : IEqualityComparer<Currency>
        {
            public bool Equals(Currency? a, Currency? b) => a?.Id == b?.Id;
            public int GetHashCode(Currency x) => HashCode.Combine(x?.Id);
        }
    }
}
