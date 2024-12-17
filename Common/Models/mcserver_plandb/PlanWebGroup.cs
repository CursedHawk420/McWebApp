using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanWebGroup
{
    public int Id { get; set; }

    public string GroupName { get; set; } = null!;

    public virtual ICollection<PlanSecurity> PlanSecurities { get; set; } = new List<PlanSecurity>();

    public virtual ICollection<PlanWebGroupToPermission> PlanWebGroupToPermissions { get; set; } = new List<PlanWebGroupToPermission>();
}
