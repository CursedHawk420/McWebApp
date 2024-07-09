using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_maindb;

public partial class SrUrlSkin
{
    public string Url { get; set; } = null!;

    public string? MineSkinId { get; set; }

    public string Value { get; set; } = null!;

    public string Signature { get; set; } = null!;

    public string? SkinVariant { get; set; }
}
