using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_maindb;

public partial class SrPlayerSkin
{
    public string Uuid { get; set; } = null!;

    public string? LastKnownName { get; set; }

    public string Value { get; set; } = null!;

    public string Signature { get; set; } = null!;

    public long Timestamp { get; set; }
}
