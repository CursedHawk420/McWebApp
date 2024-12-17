using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanWebUserPreference
{
    public int Id { get; set; }

    public string Preferences { get; set; } = null!;

    public int? WebUserId { get; set; }

    public virtual PlanSecurity? WebUser { get; set; }
}
