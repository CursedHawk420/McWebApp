using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanWorldTime
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int WorldId { get; set; }

    public int ServerId { get; set; }

    public int SessionId { get; set; }

    public long SurvivalTime { get; set; }

    public long CreativeTime { get; set; }

    public long AdventureTime { get; set; }

    public long SpectatorTime { get; set; }

    public virtual PlanServer Server { get; set; } = null!;

    public virtual PlanSession Session { get; set; } = null!;

    public virtual PlanUser User { get; set; } = null!;

    public virtual PlanWorld World { get; set; } = null!;
}
