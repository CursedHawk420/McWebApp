using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_husksync;

public partial class HusksyncUser
{
    public Guid Uuid { get; set; }

    public string Username { get; set; } = null!;

    public virtual ICollection<HusksyncUserDatum> HusksyncUserData { get; set; } = new List<HusksyncUserDatum>();
}
