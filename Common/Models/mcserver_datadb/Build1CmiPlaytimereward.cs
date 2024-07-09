using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_datadb;

public partial class Build1CmiPlaytimereward
{
    public int Id { get; set; }

    public int? PlayerId { get; set; }

    public string? Repeatable { get; set; }

    public string? Onetime { get; set; }
}
