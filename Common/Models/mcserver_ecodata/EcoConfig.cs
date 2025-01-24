using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_ecodata;

public partial class EcoConfig
{
    public byte[] ProfileUuid { get; set; } = null!;

    public string DataKey { get; set; } = null!;

    public string DataValue { get; set; } = null!;
}
