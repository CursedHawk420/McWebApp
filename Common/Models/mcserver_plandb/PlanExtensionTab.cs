using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanExtensionTab
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string ElementOrder { get; set; } = null!;

    public int TabPriority { get; set; }

    public int PluginId { get; set; }

    public int IconId { get; set; }

    public virtual PlanExtensionIcon Icon { get; set; } = null!;

    public virtual ICollection<PlanExtensionProvider> PlanExtensionProviders { get; set; } = new List<PlanExtensionProvider>();

    public virtual ICollection<PlanExtensionTable> PlanExtensionTables { get; set; } = new List<PlanExtensionTable>();

    public virtual PlanExtensionPlugin Plugin { get; set; } = null!;
}
