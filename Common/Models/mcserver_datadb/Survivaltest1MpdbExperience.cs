using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_datadb;

public partial class Survivaltest1MpdbExperience
{
    public uint Id { get; set; }

    public Guid PlayerUuid { get; set; }

    public string PlayerName { get; set; } = null!;

    public float Exp { get; set; }

    public int ExpToLevel { get; set; }

    public int TotalExp { get; set; }

    public int ExpLvl { get; set; }

    public string SyncComplete { get; set; } = null!;

    public string LastSeen { get; set; } = null!;
}
