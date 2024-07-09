using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_datadb;

public partial class Atm9MpdbInventory
{
    public uint Id { get; set; }

    public Guid PlayerUuid { get; set; }

    public string PlayerName { get; set; } = null!;

    public string Inventory { get; set; } = null!;

    public string Armor { get; set; } = null!;

    public int HotbarSlot { get; set; }

    public int Gamemode { get; set; }

    public string SyncComplete { get; set; } = null!;

    public string LastSeen { get; set; } = null!;
}
