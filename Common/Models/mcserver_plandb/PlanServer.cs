using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanServer
{
    public int Id { get; set; }

    public string Uuid { get; set; } = null!;

    public string? Name { get; set; }

    public string? WebAddress { get; set; }

    public bool? IsInstalled { get; set; }

    public bool IsProxy { get; set; }

    public string? PlanVersion { get; set; }

    public virtual ICollection<PlanAllowlistBounce> PlanAllowlistBounces { get; set; } = new List<PlanAllowlistBounce>();

    public virtual ICollection<PlanPing> PlanPings { get; set; } = new List<PlanPing>();

    public virtual ICollection<PlanPluginVersion> PlanPluginVersions { get; set; } = new List<PlanPluginVersion>();

    public virtual ICollection<PlanSession> PlanSessions { get; set; } = new List<PlanSession>();

    public virtual ICollection<PlanTp> PlanTps { get; set; } = new List<PlanTp>();

    public virtual ICollection<PlanUserInfo> PlanUserInfos { get; set; } = new List<PlanUserInfo>();

    public virtual ICollection<PlanWorldTime> PlanWorldTimes { get; set; } = new List<PlanWorldTime>();
}
