using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanSetting
{
    public int Id { get; set; }

    public string ServerUuid { get; set; } = null!;

    public long Updated { get; set; }

    public string Content { get; set; } = null!;
}
