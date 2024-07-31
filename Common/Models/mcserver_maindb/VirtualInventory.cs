using Highgeek.McWebApp.Common.Models.mcserver_datadb;
using Highgeek.McWebApp.Common.Models.Minecraft;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Highgeek.McWebApp.Common.Models.mcserver_maindb;

public partial class VirtualInventory
{
    public string? PlayerUuid { get; set; }

    public string? PlayerName { get; set; }

    public string InventoryUuid { get; set; } = null!;

    public string? InventoryName { get; set; }

    public int Size { get; set; }

    public bool Web { get; set; }

    public string? Jsondata { get; set; }

    public string? LastUpdated { get; set; }

    [NotMapped]
    public List<GameItem?>? Items { get; set; }


    // Note: this is important so the select can compare InventoryData
    public override bool Equals(object o)
    {
        var other = o as VirtualInventory;
        return other?.InventoryName == InventoryName;
    }

    // Note: this is important so the select can compare InventoryData
    public override int GetHashCode() => InventoryName.GetHashCode();

}

/*public class InventoryData
{
    public VirtualInventory? VirtualInventory { get; set; }
    public List<GameItem?>? Items { get; set; }

}*/

public class InventoriesList
{
    public List<VirtualInventory> Inventories { get; set; }

    public Dictionary<string, int> ListPosition { get; } = new Dictionary<string, int>();

    public VirtualInventory WInvData;

    public InventoriesList(List<VirtualInventory> inventories)
    {
        Inventories = inventories;
        int i = 0;

        foreach (var inv in Inventories)
        {
            ListPosition.Add(inv.InventoryUuid, i);
            i = i + inv.Size;
            if (inv.Web)
            {
                WInvData = inv;
            }
        }
    }

}