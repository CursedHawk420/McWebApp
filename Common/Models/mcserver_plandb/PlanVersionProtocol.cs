using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanVersionProtocol
{
    public int Id { get; set; }

    public string Uuid { get; set; } = null!;

    public int ProtocolVersion { get; set; }
}
