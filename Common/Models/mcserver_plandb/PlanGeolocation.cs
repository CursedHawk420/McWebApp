using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanGeolocation
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Geolocation { get; set; } = null!;

    public long LastUsed { get; set; }

    public virtual PlanUser User { get; set; } = null!;
}
