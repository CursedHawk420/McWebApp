using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcwebapp1_cms;

[PrimaryKey(nameof(Id))]
public partial class CarouselContent
{
    public string? Header { get; set; }

    public string? Content { get; set; }

    public string? Imageurl { get; set; }

    public int Order { get; set; }

    public bool? Visible { get; set; }

    public int Id { get; set; }
}
