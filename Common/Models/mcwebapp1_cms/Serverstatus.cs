using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcwebapp1_cms;

public partial class Serverstatus
{
    public string Name { get; set; } = null!;

    public string? Players { get; set; }

    public string? Maxplayers { get; set; }

    public List<string>? Playerslist { get; set; }

    public string? Online { get; set; }

    public string Order { get; set; } = null!;

    public bool? Visible { get; set; }

    public bool? Maintenance { get; set; }

    public string? Ip { get; set; }

    public string? Port { get; set; }

    public string? Rconport { get; set; }

    public string? Rconpass { get; set; }
}
