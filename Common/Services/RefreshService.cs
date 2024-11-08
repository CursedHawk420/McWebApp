using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Highgeek.McWebApp.Common.Services
{
    public interface IRefreshService
    {
        event Action PageRefreshRequested;
        void CallPageRefresh();

        event Action ServiceRefreshRequested;
        void CallServiceRefresh();

        event Action ApplicationUserRefreshRequested;
        void CallApplicationUserRefresh();

        event Action ChatWindowCloseRequested;
        void CallChatWindowClose();

        event Action ChatRefreshRequested;
        void CallChatRefresh();

        event Action ChatServiceRefreshRequested;
        void CallChatServiceRefresh();

        event Action MainNavMenuClosehRequested;
        void CallMainNavMenuClose();

        event Action InventoryServiceRefreshRequested;
        void CallInventoryServiceRefresh();

        event Action InventoryViewRefreshRequested;
        void CallInventoryViewRefresh();

        event Action LanguageProviderRefreshRequested;
        void CallLanguageProviderRefresh();

        public event Action EcoRefreshRequested;
        public void CallEcoRefresh();
    }
    public class RefreshService : IRefreshService
    {

        public event Action LanguageProviderRefreshRequested;
        public void CallLanguageProviderRefresh()
        {
            LanguageProviderRefreshRequested?.Invoke();
        }

        public event Action PageRefreshRequested;
        public void CallPageRefresh()
        {
            PageRefreshRequested?.Invoke();
        }

        public event Action InventoryServiceRefreshRequested;
        public void CallInventoryServiceRefresh()
        {
            InventoryServiceRefreshRequested?.Invoke();
        }

        public event Action InventoryViewRefreshRequested;
        public void CallInventoryViewRefresh()
        {
            InventoryViewRefreshRequested?.Invoke();
        }


        public event Action ServiceRefreshRequested;
        public void CallServiceRefresh()
        {
            ServiceRefreshRequested?.Invoke();
        }


        public event Action ChatRefreshRequested;
        public void CallChatRefresh()
        {
            ChatRefreshRequested?.Invoke();
        }


        public event Action ChatServiceRefreshRequested;
        public void CallChatServiceRefresh()
        {
            ChatServiceRefreshRequested?.Invoke();
        }


        public event Action ApplicationUserRefreshRequested;
        public void CallApplicationUserRefresh()
        {
            ApplicationUserRefreshRequested?.Invoke();
        }

        public event Action ChatWindowCloseRequested;
        public void CallChatWindowClose()
        {
            ChatWindowCloseRequested?.Invoke();
        }

        public event Action MainNavMenuClosehRequested;
        public void CallMainNavMenuClose()
        {
            MainNavMenuClosehRequested?.Invoke();
        }

        public event Action EcoRefreshRequested;
        public void CallEcoRefresh()
        {
            EcoRefreshRequested?.Invoke();
        }
    }
}