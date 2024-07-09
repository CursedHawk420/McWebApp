using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcwebapp1_cms;

public partial class ImageCache
{
    public string Uuid { get; set; } = null!;

    public string? Name { get; set; }

    public string? Imageurl { get; set; }

    public byte[] Image { get; set; } = null!;

    public string? Format { get; set; }

    public string Date { get; set; } = null!;
}
