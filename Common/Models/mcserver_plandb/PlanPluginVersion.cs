using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanPluginVersion
{
    public int Id { get; set; }

    public int ServerId { get; set; }

    public string PluginName { get; set; } = null!;

    public string? Version { get; set; }

    public long Modified { get; set; }

    public virtual PlanServer Server { get; set; } = null!;
}
