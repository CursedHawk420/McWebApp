using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_maindb;

public partial class DiscordsrvAccount
{
    public int Link { get; set; }

    public string Discord { get; set; } = null!;

    public string Uuid { get; set; } = null!;
}
