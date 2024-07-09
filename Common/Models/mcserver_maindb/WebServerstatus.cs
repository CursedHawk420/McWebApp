using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_maindb;

[PrimaryKey(nameof(Name))]
public partial class WebServerstatus
{
    public string Name { get; set; } = null!;

    public string? Players { get; set; }

    public string? Maxplayers { get; set; }

    public string? Online { get; set; }

    public List<string>? PlayersList { get; set; }

    public string Order { get; set; }
}
