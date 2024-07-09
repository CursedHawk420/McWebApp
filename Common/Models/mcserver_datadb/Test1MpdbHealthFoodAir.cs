using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_datadb;

public partial class Test1MpdbHealthFoodAir
{
    public uint Id { get; set; }

    public Guid PlayerUuid { get; set; }

    public string PlayerName { get; set; } = null!;

    public double Health { get; set; }

    public double HealthScale { get; set; }

    public double MaxHealth { get; set; }

    public int Food { get; set; }

    public string Saturation { get; set; } = null!;

    public int Air { get; set; }

    public int MaxAir { get; set; }

    public string SyncComplete { get; set; } = null!;

    public string LastSeen { get; set; } = null!;
}
