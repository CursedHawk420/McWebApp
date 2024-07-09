using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_maindb;

public partial class Xconomyrecord
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public string Uid { get; set; } = null!;

    public string Player { get; set; } = null!;

    public double? Balance { get; set; }

    public double Amount { get; set; }

    public string Operation { get; set; } = null!;

    public string Command { get; set; } = null!;

    public string Comment { get; set; } = null!;

    public DateTime Datetime { get; set; }
}
