using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanWorld
{
    public int Id { get; set; }

    public string WorldName { get; set; } = null!;

    public string ServerUuid { get; set; } = null!;

    public virtual ICollection<PlanWorldTime> PlanWorldTimes { get; set; } = new List<PlanWorldTime>();
}
