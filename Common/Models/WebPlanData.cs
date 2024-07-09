using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models;

public partial class WebPlanData
{
    public string? Uuid { get; set; }

    public string Name { get; set; } = null!;

    public string? Plandata { get; set; }
}
