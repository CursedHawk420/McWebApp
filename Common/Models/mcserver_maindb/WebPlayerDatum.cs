using Highgeek.McWebApp.Common.Data.Plan;
using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_maindb;

public partial class WebPlayerDatum
{
    public string? Uuid { get; set; }

    public string Name { get; set; } = null!;

    public string? Plandata { get; set; }

}

public partial class WebPlayerDatumEnhanced
{
    public WebPlayerDatum WebPlayerDatum { get; set; }

    public PlanApiPlayerDeserializer? PlanDataDeserialized { get; set; }
}
