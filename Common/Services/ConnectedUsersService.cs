using Highgeek.McWebApp.Common.Models;
using Highgeek.McWebApp.Common.Services.Redis;
using Newtonsoft.Json;

namespace Highgeek.McWebApp.Common.Services
{
    public interface IConnectedUsersService
    {
        public string TenantId { get; set; }

        public List<IUserService> Users { get; set; }

        public List<Session> GlobalSessions { get; set; }

        public IList<IUserService> FindSessionsByUser(string userid);
        public void DisconnectMcAccount(object? sender,string userid);

        public event EventHandler<IUserService> SessionAdded;
        public void CallSessionAdded(IUserService userService);

        public event EventHandler<IUserService> SessionRemoved;
        public void CallSessionRemoved(IUserService userService);

        public void AddSession(IUserService user);
        public void RemoveSession(IUserService user);

        public event Action AdminViewRefreshRequested;
        public void CallAdminViewRefresh();

    }
    public class ConnectedUsersService : IConnectedUsersService, IDisposable
    {
        private readonly IRedisUpdateService _redisUpdateService;
        private bool disposedValue;

        public ConnectedUsersService(IRedisUpdateService redisUpdateService)
        {
            _redisUpdateService = redisUpdateService;
            Users = new List<IUserService>();
            GlobalSessions = new List<Session>();

            TenantId = Environment.GetEnvironmentVariable("TENANT_ID");

            _redisUpdateService.McAccountDisconnectUpdate += DisconnectMcAccount;
            _redisUpdateService.SessionListUpdate += UpdateSessionsFromRedis;
        }

        public string TenantId { get; set; }

        public List<Session> GlobalSessions { get; set; }

        public List<IUserService> Users { get; set; }

        public IList<IUserService> FindSessionsByUser(string userid)
        {
            return Users.FindAll(x => x.ApplicationUser.Id == userid);
        }

        public async void DisconnectMcAccount(object? sender, string userid)
        {
            var list = FindSessionsByUser(userid);
            if (list.Count > 0)
            {
                foreach (var userservice in list)
                {
                    await userservice.DisconnectGameAccount();
                    userservice._refreshService.CallServiceRefresh();
                }
            }
            CallAdminViewRefresh();
        }

        public async void AddSession(IUserService user)
        {
            await AddSessiontoRedis(user);
            Users.Add(user);
            this.CallSessionAdded(user);
        }

        public async void RemoveSession(IUserService user)
        {
            await RemoveSessionFromRedis(user);
            Users.Remove(user);
            this.CallSessionRemoved(user);
        }

        public async Task AddSessiontoRedis(IUserService user)
        {
            string data = RedisService.GetFromRedis("appchannel:mcwebapp:sessionlist");
            List<Session> sessions = new List<Session>(JsonConvert.DeserializeObject<List<Session>>(data));
            string username = "";
            string email = "";
            if (user.ApplicationUser != null)
            {
                username = user.ApplicationUser.UserName;
                email = user.ApplicationUser.Email;
            }
            sessions.Add(new Session(user.ServiceId, username, email, TenantId));

            await RedisService.SetInRedis("appchannel:mcwebapp:sessionlist", JsonConvert.SerializeObject(sessions));
        }

        public async Task RemoveSessionFromRedis(IUserService user)
        {
            string data = RedisService.GetFromRedis("appchannel:mcwebapp:sessionlist");
            var json = JsonConvert.DeserializeObject<List<Session>>(data);


            var itemToRemove = json.SingleOrDefault(r => r.Id == user.ServiceId);
            json.Remove(itemToRemove);

            await RedisService.SetInRedis("appchannel:mcwebapp:sessionlist", JsonConvert.SerializeObject(json));
        }


        public async void UpdateSessionsFromRedis(object? sender, string sessionJson)
        {
            string data = RedisService.GetFromRedis("appchannel:mcwebapp:sessionlist");
            GlobalSessions = JsonConvert.DeserializeObject<List<Session>>(data);
            CallAdminViewRefresh();
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach(var user in Users)
                    {
                        RemoveSession(user);
                    }
                    // TODO: Uvolněte spravovaný stav (spravované objekty).
                }

                // TODO: Uvolněte nespravované prostředky (nespravované objekty) a přepište finalizační metodu.
                // TODO: Nastavte velká pole na hodnotu null.
                disposedValue = true;
            }
        }

        // // TODO: Finalizační metodu přepište, jen pokud metoda Dispose(bool disposing) obsahuje kód pro uvolnění nespravovaných prostředků.
        // ~ConnectedUsersService()
        // {
        //     // Neměňte tento kód. Kód pro vyčištění vložte do metody Dispose(bool disposing).
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Neměňte tento kód. Kód pro vyčištění vložte do metody Dispose(bool disposing).
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    public class Session
    {
        public Session(string id, string? UserName, string? Email, string tenantID)
        {
            this.UserName = UserName;
            this.Email = Email;
            this.Id = id;
            TenantID = tenantID;
        }

        public string Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string TenantID { get; set; }
    }

}
