using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanExtensionIcon
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Family { get; set; } = null!;

    public string Color { get; set; } = null!;

    public virtual ICollection<PlanExtensionPlugin> PlanExtensionPlugins { get; set; } = new List<PlanExtensionPlugin>();

    public virtual ICollection<PlanExtensionProvider> PlanExtensionProviders { get; set; } = new List<PlanExtensionProvider>();

    public virtual ICollection<PlanExtensionTable> PlanExtensionTableIcon1s { get; set; } = new List<PlanExtensionTable>();

    public virtual ICollection<PlanExtensionTable> PlanExtensionTableIcon2s { get; set; } = new List<PlanExtensionTable>();

    public virtual ICollection<PlanExtensionTable> PlanExtensionTableIcon3s { get; set; } = new List<PlanExtensionTable>();

    public virtual ICollection<PlanExtensionTable> PlanExtensionTableIcon4s { get; set; } = new List<PlanExtensionTable>();

    public virtual ICollection<PlanExtensionTable> PlanExtensionTableIcon5s { get; set; } = new List<PlanExtensionTable>();

    public virtual ICollection<PlanExtensionTab> PlanExtensionTabs { get; set; } = new List<PlanExtensionTab>();
}
