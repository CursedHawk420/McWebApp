using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_ecodata;

public partial class EcoBoolean
{
    public byte[] ProfileUuid { get; set; } = null!;

    public string DataKey { get; set; } = null!;

    public bool DataValue { get; set; }
}
