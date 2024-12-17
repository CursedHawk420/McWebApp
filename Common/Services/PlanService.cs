using Highgeek.McWebApp.Common.Models.Contexts;
using Highgeek.McWebApp.Common.Models.mcserver_plandb;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highgeek.McWebApp.Common.Services
{
    public interface IPlanService
    {
        public List<PlanKill> GetPlayerKills(string playerUuid);
        public List<PlanKill> GetPlayerDeaths(string playerUuid);
        public PlanUser GetPlanPlayer(string playerUuid);
    }

    public class PlanService : IPlanService
    {
        private readonly McserverPlandbContext _planDb = new McserverPlandbContext();

        public PlanService()
        {
        }

        public List<PlanKill> GetPlayerKills(string playerUuid)
        {
            return _planDb.PlanKills.Where(r => r.KillerUuid == playerUuid).ToList();
        }


        public List<PlanKill> GetPlayerDeaths(string playerUuid)
        {
            return _planDb.PlanKills.Where(r => r.VictimUuid == playerUuid).ToList();
        }

        public PlanUser GetPlanPlayer(string playerUuid)
        {
            return _planDb.PlanUsers
                .Include(u => u.PlanSessions.OrderByDescending(x => x.SessionStart).Take(10))
                //.Include(u => u.PlanUserInfos)
                //.Include(u => u.PlanPings)
                //.Include(u => u.PlanGeolocations)
                .FirstOrDefault(r => r.Uuid == playerUuid);
        }
    }
}
