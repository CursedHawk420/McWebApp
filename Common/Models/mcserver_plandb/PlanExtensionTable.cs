using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanExtensionTable
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Color { get; set; } = null!;

    public int? ValuesFor { get; set; }

    public string? ConditionName { get; set; }

    public string? Col1Name { get; set; }

    public string? Col2Name { get; set; }

    public string? Col3Name { get; set; }

    public string? Col4Name { get; set; }

    public string? Col5Name { get; set; }

    public int PluginId { get; set; }

    public int? Icon1Id { get; set; }

    public int? Icon2Id { get; set; }

    public int? Icon3Id { get; set; }

    public int? Icon4Id { get; set; }

    public int? Icon5Id { get; set; }

    public string? Format1 { get; set; }

    public string? Format2 { get; set; }

    public string? Format3 { get; set; }

    public string? Format4 { get; set; }

    public string? Format5 { get; set; }

    public int? TabId { get; set; }

    public virtual PlanExtensionIcon? Icon1 { get; set; }

    public virtual PlanExtensionIcon? Icon2 { get; set; }

    public virtual PlanExtensionIcon? Icon3 { get; set; }

    public virtual PlanExtensionIcon? Icon4 { get; set; }

    public virtual PlanExtensionIcon? Icon5 { get; set; }

    public virtual ICollection<PlanExtensionServerTableValue> PlanExtensionServerTableValues { get; set; } = new List<PlanExtensionServerTableValue>();

    public virtual ICollection<PlanExtensionUserTableValue> PlanExtensionUserTableValues { get; set; } = new List<PlanExtensionUserTableValue>();

    public virtual PlanExtensionPlugin Plugin { get; set; } = null!;

    public virtual PlanExtensionTab? Tab { get; set; }
}
