using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanExtensionPlugin
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public long LastUpdated { get; set; }

    public string ServerUuid { get; set; } = null!;

    public int IconId { get; set; }

    public virtual PlanExtensionIcon Icon { get; set; } = null!;

    public virtual ICollection<PlanExtensionProvider> PlanExtensionProviders { get; set; } = new List<PlanExtensionProvider>();

    public virtual ICollection<PlanExtensionTable> PlanExtensionTables { get; set; } = new List<PlanExtensionTable>();

    public virtual ICollection<PlanExtensionTab> PlanExtensionTabs { get; set; } = new List<PlanExtensionTab>();
}
