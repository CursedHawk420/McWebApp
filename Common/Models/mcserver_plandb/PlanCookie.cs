using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanCookie
{
    public int Id { get; set; }

    public string WebUsername { get; set; } = null!;

    public long Expires { get; set; }

    public string Cookie { get; set; } = null!;
}
