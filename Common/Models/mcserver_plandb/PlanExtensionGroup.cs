using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanExtensionGroup
{
    public int Id { get; set; }

    public string Uuid { get; set; } = null!;

    public string? GroupName { get; set; }

    public int ProviderId { get; set; }

    public virtual PlanExtensionProvider Provider { get; set; } = null!;
}
