using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_husksync;

public partial class HusksyncUserDatum
{
    public Guid VersionUuid { get; set; }

    public Guid PlayerUuid { get; set; }

    public DateTime Timestamp { get; set; }

    public string SaveCause { get; set; } = null!;

    public bool Pinned { get; set; }

    public byte[] Data { get; set; } = null!;

    public virtual HusksyncUser PlayerUu { get; set; } = null!;
}
