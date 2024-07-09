using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_maindb;

public partial class Xconomy
{
    public string Uid { get; set; } = null!;

    public string Player { get; set; } = null!;

    public double Balance { get; set; }

    public int Hidden { get; set; }
}
