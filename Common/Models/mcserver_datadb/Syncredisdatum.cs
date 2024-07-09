using System;
using System.Collections.Generic;
using Highgeek.McWebApp.Common.Models.Minecraft;

namespace Highgeek.McWebApp.Common.Models.mcserver_datadb;

public partial class Syncredisdatum
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
        var other = o as Syncredisdatum;
        return other?.InventoryName == InventoryName;
    }

    // Note: this is important so the select can compare InventoryData
    public override int GetHashCode() => InventoryName.GetHashCode();

}

public class InventoryData
{
    public Syncredisdatum? Syncredisdatum { get; set; }
    public List<GameItem?>? Items { get; set; }

}
