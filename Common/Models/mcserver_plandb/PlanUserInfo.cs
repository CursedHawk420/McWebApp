using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanUserInfo
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ServerId { get; set; }

    public string? JoinAddress { get; set; }

    public long Registered { get; set; }

    public bool Opped { get; set; }

    public bool Banned { get; set; }

    public virtual PlanServer Server { get; set; } = null!;

    public virtual PlanUser User { get; set; } = null!;
}
