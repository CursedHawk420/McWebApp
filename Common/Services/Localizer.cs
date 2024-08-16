using Highgeek.McWebApp.Common.Services.Redis;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highgeek.McWebApp.Common.Services
{
    public interface ILocalizer : IDisposable
    {
        string this[string name] { get; }

        public string Locale { get; set; }


        event Action LocaleRefreshRequested;
        void CallLocaleRefresh();
    }

    public class Localizer : ILocalizer
    {
        private readonly ILanguageProvider _languageProvider;

        private readonly IRedisUpdateService _redisUpdateService;
        private bool disposedValue;

        public string Locale {  get; set; }
        
        public Localizer(ILanguageProvider languageProvider, IRedisUpdateService redisUpdateService)
        {
            _languageProvider = languageProvider;
            _redisUpdateService = redisUpdateService;
            _redisUpdateService.LanguageProviderRefreshRequested += CallLocaleRefresh;
        }

        public virtual string this[string name]
        {
            get
            {
                return _languageProvider.GetLocale(name, Locale);
            }
        }


        public event Action LocaleRefreshRequested;
        public void CallLocaleRefresh()
        {
            LocaleRefreshRequested?.Invoke();
        }







        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: Uvolněte spravovaný stav (spravované objekty).
                    _redisUpdateService.LanguageProviderRefreshRequested -= CallLocaleRefresh;
                }

                // TODO: Uvolněte nespravované prostředky (nespravované objekty) a přepište finalizační metodu.
                // TODO: Nastavte velká pole na hodnotu null.
                disposedValue = true;
            }
        }

        // // TODO: Finalizační metodu přepište, jen pokud metoda Dispose(bool disposing) obsahuje kód pro uvolnění nespravovaných prostředků.
         ~Localizer()
         {
             // Neměňte tento kód. Kód pro vyčištění vložte do metody Dispose(bool disposing).
             Dispose(disposing: false);
         }

        public void Dispose()
        {
            // Neměňte tento kód. Kód pro vyčištění vložte do metody Dispose(bool disposing).
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
