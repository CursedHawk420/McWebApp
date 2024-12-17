using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanSecurity
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string? LinkedToUuid { get; set; }

    public string SaltedPassHash { get; set; } = null!;

    public int GroupId { get; set; }

    public virtual PlanWebGroup Group { get; set; } = null!;

    public virtual ICollection<PlanWebUserPreference> PlanWebUserPreferences { get; set; } = new List<PlanWebUserPreference>();
}
