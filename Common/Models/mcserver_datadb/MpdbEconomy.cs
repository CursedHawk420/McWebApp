using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_datadb;

public partial class MpdbEconomy
{
    public uint Id { get; set; }

    public Guid PlayerUuid { get; set; }

    public string PlayerName { get; set; } = null!;

    public double Money { get; set; }

    public double OfflineMoney { get; set; }

    public string SyncComplete { get; set; } = null!;

    public string LastSeen { get; set; } = null!;
}
