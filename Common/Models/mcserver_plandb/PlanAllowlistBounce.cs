using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanAllowlistBounce
{
    public int Id { get; set; }

    public string Uuid { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int ServerId { get; set; }

    public int Times { get; set; }

    public long LastBounce { get; set; }

    public virtual PlanServer Server { get; set; } = null!;
}
