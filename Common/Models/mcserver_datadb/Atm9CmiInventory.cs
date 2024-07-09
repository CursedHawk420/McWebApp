using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_datadb;

public partial class Atm9CmiInventory
{
    public int Id { get; set; }

    public int? PlayerId { get; set; }

    public string? Inventories { get; set; }
}
