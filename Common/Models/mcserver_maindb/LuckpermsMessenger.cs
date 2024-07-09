using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_maindb;

public partial class LuckpermsMessenger
{
    public int Id { get; set; }

    public DateTime Time { get; set; }

    public string Msg { get; set; } = null!;
}
