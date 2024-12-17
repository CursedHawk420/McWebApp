using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanExtensionServerTableValue
{
    public int Id { get; set; }

    public string Uuid { get; set; } = null!;

    public int TableRow { get; set; }

    public string? Col1Value { get; set; }

    public string? Col2Value { get; set; }

    public string? Col3Value { get; set; }

    public string? Col4Value { get; set; }

    public string? Col5Value { get; set; }

    public int TableId { get; set; }

    public virtual PlanExtensionTable Table { get; set; } = null!;
}
