using Highgeek.McWebApp.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highgeek.McWebApp.Common.Models
{
    public class ApplicationUserData
    {
        public ApplicationUser ApplicationUser { get; set; }

        public MinecraftUser? MinecraftUser { get; set; }

        public bool ShowDetails = false;

        public bool HasConnectedAccount = false;

        public Dictionary<string, float>? Economy { get; set; }


    }
}
