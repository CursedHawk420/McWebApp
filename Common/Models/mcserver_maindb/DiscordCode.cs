using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_maindb;

public partial class DiscordCode
{
    public string Code { get; set; } = null!;

    public string Uuid { get; set; } = null!;

    public long Expiration { get; set; }
}
