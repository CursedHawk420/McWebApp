using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanPing
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ServerId { get; set; }

    public long Date { get; set; }

    public int MaxPing { get; set; }

    public int MinPing { get; set; }

    public double AvgPing { get; set; }

    public virtual PlanServer Server { get; set; } = null!;

    public virtual PlanUser User { get; set; } = null!;
}
