using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanExtensionProvider
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Text { get; set; } = null!;

    public string? Description { get; set; }

    public int Priority { get; set; }

    public bool ShowInPlayersTable { get; set; }

    public bool Groupable { get; set; }

    public string? ConditionName { get; set; }

    public string? ProvidedCondition { get; set; }

    public string? FormatType { get; set; }

    public bool Hidden { get; set; }

    public bool PlayerName { get; set; }

    public int PluginId { get; set; }

    public int IconId { get; set; }

    public int? TabId { get; set; }

    public virtual PlanExtensionIcon Icon { get; set; } = null!;

    public virtual ICollection<PlanExtensionGroup> PlanExtensionGroups { get; set; } = new List<PlanExtensionGroup>();

    public virtual ICollection<PlanExtensionServerValue> PlanExtensionServerValues { get; set; } = new List<PlanExtensionServerValue>();

    public virtual ICollection<PlanExtensionUserValue> PlanExtensionUserValues { get; set; } = new List<PlanExtensionUserValue>();

    public virtual PlanExtensionPlugin Plugin { get; set; } = null!;

    public virtual PlanExtensionTab? Tab { get; set; }
}
