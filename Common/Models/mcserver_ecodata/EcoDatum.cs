using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_ecodata;

public partial class EcoDatum
{
    public byte[] Id { get; set; } = null!;

    public string JsonData { get; set; } = null!;
}
