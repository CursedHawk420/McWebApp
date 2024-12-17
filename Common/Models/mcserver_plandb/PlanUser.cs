using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanUser
{
    public int Id { get; set; }

    public string Uuid { get; set; } = null!;

    public long Registered { get; set; }

    public string Name { get; set; } = null!;

    public int TimesKicked { get; set; }

    public virtual ICollection<PlanGeolocation> PlanGeolocations { get; set; } = new List<PlanGeolocation>();

    public virtual ICollection<PlanPing> PlanPings { get; set; } = new List<PlanPing>();

    public virtual ICollection<PlanSession> PlanSessions { get; set; } = new List<PlanSession>();

    public virtual ICollection<PlanUserInfo> PlanUserInfos { get; set; } = new List<PlanUserInfo>();

    public virtual ICollection<PlanWorldTime> PlanWorldTimes { get; set; } = new List<PlanWorldTime>();
}
