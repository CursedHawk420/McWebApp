using System.Collections.Generic;
using System.Diagnostics;
using Highgeek.McWebApp.Common.Helpers;
using LuckPermsApi.Api;
using LuckPermsApi.Client;
using LuckPermsApi.Model;

namespace Highgeek.McWebApp.Common.Services
{
    public class LuckPermsService
    {
        private Configuration config = new Configuration();
        public readonly UsersApi _usersApi;
        public readonly GroupsApi _groupApi;

        private readonly ConfigProvider configProvider = ConfigProvider.Instance;

        public LuckPermsService()
        {
            config.BasePath = configProvider.GetConfigString("LuckPermsOptions:Url");
            _usersApi = new UsersApi(config);
            _groupApi = new GroupsApi(config);
        }

        public async Task<UsersApi> GetUsersApiAsync()
        {
            return new UsersApi(config);
        }

        public async Task<GroupsApi> GetGroupsApiAsync()
        {
            return new GroupsApi(config); 
        }

        public async Task<string> GetUserUuidAsync(string username)
        {
            var api = await GetUsersApiAsync();
            var result = await api.GetUserLookupAsync(username);
            api = null;
            return result.UniqueId.ToString();
        }

        public async Task<User> GetUserAsync(string uuid)
        {
            var api = await GetUsersApiAsync();
            var result = await api.GetUserAsync(new Guid(uuid));
            api = null;
            return result;
        }

        public async Task<Group> GetGroupAsync(string groupname)
        {
            var api = await GetGroupsApiAsync();
            var result = await api.GetGroupAsync(groupname);
            api = null;
            return result;
        }

        public async Task<List<string>> SearchForNodeInGroupsAsync(string permission)
        {
            var api = await GetGroupsApiAsync();
            var result = await api.GetGroupSearchAsync(permission);
            api = null;
            List<string> list = new List<string>();
            foreach (var item in result)
            {
                foreach (var results in item.Results)
                {
                    if (results.Value == true)
                    {
                        list.Add(item.Name);
                    }
                }
            }
            return list;
        }

        public async Task<List<string>> SearchForNodeInUserAsync(string permission)
        {
            var api = await GetUsersApiAsync();
            var result = await api.GetUserSearchAsync(permission);
            api = null;
            List<string> list = new List<string>();
            foreach (var item in result)
            {
                list.Add(item.UniqueId.ToString());
            }
            return list;
        }

        public async Task<bool> HasUserNode(string permission, string uuid)
        {
            var api = await GetUsersApiAsync();
            var result = await api.GetUserPermissionCheckAsync(new Guid(uuid), permission);
            api = null;
            if (result.Node != null)
            { 
                return result.Node.Value;
            } 
            else 
            {
                return false; 
            }
        }

        public async Task<bool> HasGroupNode(string permission, string groupName)
        {
            var api = await GetGroupsApiAsync();
            var result = await api.GetGroupPermissionCheckAsync(groupName, permission);
            api = null;
            return result.Node.Value;
        }

        public async Task<bool> HasPermissionAsync(string permission, string uuid)
        {
            if(await HasUserNode(permission, uuid))
            {
                return true;
            }
            else
            {
                var groupList = await GetUserGroupsAsync(uuid);
                foreach (var group in groupList)
                {
                    if ("group." + group == permission)
                    {
                        return true;
                    }
                    else
                    {
                        var groups = await SearchForNodeInGroupsAsync(permission);
                        if (groups.Contains(group))
                        {  
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        public async Task<List<string>> GetUserGroupsAsync(string uuid)
        {
            List<string> userGroups = new List<string>();
            var user = await GetUserAsync(uuid);
            return user.ParentGroups;
        }

        public async Task<bool> CheckDefaultPerms(string permission)
        {
            var api = await GetGroupsApiAsync();
            var group = await api.GetGroupAsync("default");
            api = null;
            foreach (var item in group.Nodes)
            {
                if(item.Key == permission)
                {
                    return item.Value;
                }
            }
            return false;
        }
    }
}
