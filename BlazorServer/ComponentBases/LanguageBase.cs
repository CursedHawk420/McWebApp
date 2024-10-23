using Highgeek.McWebApp.Common.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using OpenApi.Highgeek.LuckPermsApi.Model;

namespace Highgeek.McWebApp.BlazorServer.ComponentBases
{
    public class LanguageBase : ComponentBase, IDisposable
    {
        [Inject]
        public ILocalizer l {  get; set; }

        public bool _disposed = false;

        protected override void OnInitialized()
        {
            l.LocaleRefreshRequested += RefreshLanguageAsync;
        }

        public async void RefreshLanguageAsync()
        {
            // InvokeAsync is inherited, it syncs the call back to the render thread
            await InvokeAsync(() =>
            {
                StateHasChanged();
            });
        }


        void IDisposable.Dispose()
        {
            // Dispose of unmanaged resources.
            if (!_disposed)
            {
                Dispose(true);
            }
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                l.LocaleRefreshRequested -= RefreshLanguageAsync;
            }
        }
    }
}
