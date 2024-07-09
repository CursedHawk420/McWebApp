using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_datadb;

public partial class Build1MpdbPotionEffect
{
    public uint Id { get; set; }

    public Guid PlayerUuid { get; set; }

    public string PlayerName { get; set; } = null!;

    public string PotionEffects { get; set; } = null!;

    public string SyncComplete { get; set; } = null!;

    public string LastSeen { get; set; } = null!;
}
