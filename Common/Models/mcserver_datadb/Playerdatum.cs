using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_datadb;

public partial class Playerdatum
{
    public string PlayerUuid { get; set; } = null!;

    public string? PlayerName { get; set; }

    public string? Inventory { get; set; }

    public string? Gamemode { get; set; }

    public int? Health { get; set; }

    public int? Food { get; set; }

    public string? Enderchest { get; set; }

    public int? Exp { get; set; }

    public string? LastJoined { get; set; }

    public string? Effects { get; set; }

    public string? Advancements { get; set; }

    public string? Statistics { get; set; }
}
