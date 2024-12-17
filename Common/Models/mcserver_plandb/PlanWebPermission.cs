using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanWebPermission
{
    public int Id { get; set; }

    public string Permission { get; set; } = null!;

    public virtual ICollection<PlanWebGroupToPermission> PlanWebGroupToPermissions { get; set; } = new List<PlanWebGroupToPermission>();
}
