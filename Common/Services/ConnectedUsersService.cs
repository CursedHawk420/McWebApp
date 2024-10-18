using Highgeek.McWebApp.Common.Models;
using Highgeek.McWebApp.Common.Services.Redis;

namespace Highgeek.McWebApp.Common.Services
{
    public interface IConnectedUsersService
    {

        public List<IUserService> Users { get; set; }

        public IList<IUserService> FindSessionsByUser(ApplicationUser user);

        public event EventHandler<IUserService> SessionAdded;
        public void CallSessionAdded(IUserService userService);

        public event EventHandler<IUserService> SessionRemoved;
        public void CallSessionRemoved(IUserService userService);

        public void AddSession(IUserService user);
        public void RemoveSession(IUserService user);

        public event Action AdminViewRefreshRequested;
        public void CallAdminViewRefresh();

    }
    public class ConnectedUsersService : IConnectedUsersService
    {
        public ConnectedUsersService()
        {
            Users = new List<IUserService>();
        }


        public List<IUserService> Users { get; set; }

        public IList<IUserService> FindSessionsByUser(ApplicationUser user)
        {
            return Users.FindAll(x => x.ApplicationUser.Id == user.Id);
        }

        public void AddSession(IUserService user)
        {
            Users.Add(user);
            this.CallSessionAdded(user);
        }

        public void RemoveSession(IUserService user)
        {
            Users.Remove(user);
            this.CallSessionRemoved(user);
        }



        public event EventHandler<IUserService> SessionAdded;
        public void CallSessionAdded(IUserService userService)
        {
            SessionAdded?.Invoke(this, userService);
        }
        public event EventHandler<IUserService> SessionRemoved;
        public void CallSessionRemoved(IUserService userService)
        {
            SessionRemoved?.Invoke(this, userService);
        }
        public event Action AdminViewRefreshRequested;
        public void CallAdminViewRefresh()
        {
            AdminViewRefreshRequested?.Invoke();
        }
    }
}
