using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanTp
{
    public int Id { get; set; }

    public int ServerId { get; set; }

    public long Date { get; set; }

    public double Tps { get; set; }

    public int PlayersOnline { get; set; }

    public double CpuUsage { get; set; }

    public long RamUsage { get; set; }

    public int Entities { get; set; }

    public int ChunksLoaded { get; set; }

    public long FreeDiskSpace { get; set; }

    public virtual PlanServer Server { get; set; } = null!;
}
