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

        event Action ChatWindowClosehRequested;
        void CallChatWindowClose();

        event Action MainNavMenuClosehRequested;
        void CallMainNavMenuClose();
    }
    public class RefreshService : IRefreshService
    {

        public event Action PageRefreshRequested;
        public void CallPageRefresh()
        {
            PageRefreshRequested?.Invoke();
        }


        public event Action ServiceRefreshRequested;
        public void CallServiceRefresh()
        {
            ServiceRefreshRequested?.Invoke();
        }


        public event Action ApplicationUserRefreshRequested;
        public void CallApplicationUserRefresh()
        {
            ApplicationUserRefreshRequested?.Invoke();
        }

        public event Action ChatWindowClosehRequested;
        public void CallChatWindowClose()
        {
            ChatWindowClosehRequested?.Invoke();
        }

        public event Action MainNavMenuClosehRequested;
        public void CallMainNavMenuClose()
        {
            MainNavMenuClosehRequested?.Invoke();
        }
    }
}