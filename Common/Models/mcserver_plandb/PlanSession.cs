using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanSession
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ServerId { get; set; }

    public long SessionStart { get; set; }

    public long SessionEnd { get; set; }

    public int MobKills { get; set; }

    public int Deaths { get; set; }

    public long AfkTime { get; set; }

    public int JoinAddressId { get; set; }

    public virtual ICollection<PlanKill> PlanKills { get; set; } = new List<PlanKill>();

    public virtual ICollection<PlanWorldTime> PlanWorldTimes { get; set; } = new List<PlanWorldTime>();

    public virtual PlanServer Server { get; set; } = null!;

    public virtual PlanUser User { get; set; } = null!;
}
