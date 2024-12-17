using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanNickname
{
    public int Id { get; set; }

    public string Uuid { get; set; } = null!;

    public string Nickname { get; set; } = null!;

    public string ServerUuid { get; set; } = null!;

    public long LastUsed { get; set; }
}
