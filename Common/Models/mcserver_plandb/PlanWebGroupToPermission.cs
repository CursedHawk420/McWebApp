using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_plandb;

public partial class PlanWebGroupToPermission
{
    public int Id { get; set; }

    public int GroupId { get; set; }

    public int PermissionId { get; set; }

    public virtual PlanWebGroup Group { get; set; } = null!;

    public virtual PlanWebPermission Permission { get; set; } = null!;
}
