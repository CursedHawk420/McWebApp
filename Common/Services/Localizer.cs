using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highgeek.McWebApp.Common.Services
{
    public interface ILocalizer
    {
        string this[string name] { get; }

        public string Locale { get; set; }


        event Action LocaleRefreshRequested;
        void CallLocaleRefresh();
    }

    public class Localizer : ILocalizer
    {
        private readonly ILanguageProvider _languageProvider;

        public string Locale {  get; set; }
        
        public Localizer(ILanguageProvider languageProvider, ICookieService cookieService)
        {
            _languageProvider = languageProvider;
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

    }
}
