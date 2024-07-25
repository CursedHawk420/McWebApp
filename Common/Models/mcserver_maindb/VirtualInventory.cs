using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_maindb;

public partial class VirtualInventory
{
    public string? PlayerUuid { get; set; }

    public string? PlayerName { get; set; }

    public string InventoryUuid { get; set; } = null!;

    public string? InventoryName { get; set; }

    public int? Size { get; set; }

    public bool? Web { get; set; }

    public string? Jsondata { get; set; }

    public string? LastUpdated { get; set; }
}
