using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_maindb;

public partial class LuckpermsGroupPermission
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Permission { get; set; } = null!;

    public bool Value { get; set; }

    public string Server { get; set; } = null!;

    public string World { get; set; } = null!;

    public long Expiry { get; set; }

    public string Contexts { get; set; } = null!;
}
