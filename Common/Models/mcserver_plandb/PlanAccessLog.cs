using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanAccessLog
{
    public int Id { get; set; }

    public long Time { get; set; }

    public string? FromIp { get; set; }

    public string RequestMethod { get; set; } = null!;

    public string RequestUri { get; set; } = null!;

    public int? ResponseCode { get; set; }
}
