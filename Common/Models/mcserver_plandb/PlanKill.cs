using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanKill
{
    public int Id { get; set; }

    public string KillerUuid { get; set; } = null!;

    public string VictimUuid { get; set; } = null!;

    public string ServerUuid { get; set; } = null!;

    public string Weapon { get; set; } = null!;

    public long Date { get; set; }

    public int SessionId { get; set; }

    public virtual PlanSession Session { get; set; } = null!;
}
