using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_maindb;

public partial class SrUrlIndex
{
    public string Url { get; set; } = null!;

    public string? SkinVariant { get; set; }
}
