using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_maindb;

public partial class VentureChat
{
    public ulong Id { get; set; }

    public string? ChatTime { get; set; }

    public string? Uuid { get; set; }

    public string? Name { get; set; }

    public string? Server { get; set; }

    public string? Channel { get; set; }

    public string? Text { get; set; }

    public string? Type { get; set; }
}
