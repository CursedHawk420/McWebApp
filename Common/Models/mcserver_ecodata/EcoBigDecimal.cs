using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_ecodata;

public partial class EcoBigDecimal
{
    public byte[] ProfileUuid { get; set; } = null!;

    public string DataKey { get; set; } = null!;

    public decimal DataValue { get; set; }
}
