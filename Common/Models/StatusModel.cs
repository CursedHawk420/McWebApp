using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highgeek.McWebApp.Common.Models
{
    public class StatusModel
    {
        public string? UserMessage { get; set; }
        public string? StatckTrace { get; set; }
        public bool Success { get; set; }
        public object? Object { get; set; }

        public StatusModel(string userMessage)
        {
            this.UserMessage = userMessage;
            this.Success = true;
        }

        public StatusModel(string userMessage, object Object)
        {
            this.UserMessage = userMessage;
            this.Success = true;
            this.Object = Object;
        }

        public StatusModel(string userMessage, string stackTrace)
        {
            this.UserMessage = userMessage;
            this.Success = false;
            this.StatckTrace = stackTrace;
        }
    }
}
