using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanExtensionUserValue
{
    public int Id { get; set; }

    public bool? BooleanValue { get; set; }

    public double? DoubleValue { get; set; }

    public double? PercentageValue { get; set; }

    public long? LongValue { get; set; }

    public string? StringValue { get; set; }

    public string? ComponentValue { get; set; }

    public string? GroupValue { get; set; }

    public string Uuid { get; set; } = null!;

    public int ProviderId { get; set; }

    public virtual PlanExtensionProvider Provider { get; set; } = null!;
}
