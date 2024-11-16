using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Tokero.CryptoCalculator.Web.Components.Common
{
    /// <summary>
    /// Base class razor components, that include CancellationToken when detached and logic for observable properties.
    /// </summary>
    [INotifyPropertyChanged]
    public partial class TokeroComponentBase: ComponentBase, IAsyncDisposable
    {
        private CancellationTokenSource? _cancellationTokenSource;

        /// <summary>
        /// A cancellation token that can be given to calls. Is cancelled when component is disposed.
        /// </summary>
        protected CancellationToken CancellationToken => (_cancellationTokenSource ??= new CancellationTokenSource()).Token;

        /// <summary>
        /// A cancellation token that will cancel when the component is disposed.
        /// </summary>
        protected CancellationToken ComponentDetached => CancellationToken;

        protected sealed override void OnInitialized()
        {
            PropertyChanged += OnPropertyChanged;
            base.OnInitialized();
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }

        public async ValueTask DisposeAsync()
        {
            PropertyChanged -= OnPropertyChanged;

            if (_cancellationTokenSource != null)
            {
                await _cancellationTokenSource.CancelAsync();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }

            GC.SuppressFinalize(this);
        }
    }
}
