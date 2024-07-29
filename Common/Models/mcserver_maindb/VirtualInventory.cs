using Highgeek.McWebApp.Common.Models.mcserver_datadb;
using Highgeek.McWebApp.Common.Models.Minecraft;
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


    // Note: this is important so the select can compare InventoryData
    public override bool Equals(object o)
    {
        var other = o as VirtualInventory;
        return other?.InventoryName == InventoryName;
    }

    // Note: this is important so the select can compare InventoryData
    public override int GetHashCode() => InventoryName.GetHashCode();

}

public class InventoryData
{
    public VirtualInventory? Syncredisdatum { get; set; }
    public List<GameItem?>? Items { get; set; }

}
