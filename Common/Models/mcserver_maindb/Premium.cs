using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_maindb;

public partial class Premium
{
    public int UserId { get; set; }

    public string? Uuid { get; set; }

    public string Name { get; set; } = null!;

    public bool Premium1 { get; set; }

    public string LastIp { get; set; } = null!;

    public DateTime LastLogin { get; set; }
}
