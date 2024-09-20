using Highgeek.McWebApp.Common.Helpers;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Highgeek.McWebApp.Common.Models
{
    public class StatusModel
    {
        public string? Id { get; set; }
        public string? UserMessage { get; set; }
        public string? StatckTrace { get; set; }
        public bool Success { get; set; }
        public object? Object { get; set; }
        public Exception? Exception { get; set; }
        public string? ExceptionString { get; set; }
        public string? Time {  get; set; }


        public StatusModel()
        {
            this.Success = true;
        }

        public StatusModel(string userMessage)
        {
            this.UserMessage = userMessage;
            this.Success = true;
        }

        public StatusModel(string userMessage, object Object, bool succes)
        {
            this.UserMessage = userMessage;
            this.Success = succes;
            this.Object = Object;
        }


        public StatusModel(string userMessage, Exception exception)
        {
            this.UserMessage = userMessage;
            this.Success = false;
            this.Exception = exception;
            this.ExceptionString = exception.ToJson();
            this.Id = Guid.NewGuid().ToString();

            DateTime dateTime = DateTime.UtcNow;

            this.Time = dateTime.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFF").Replace(":", "-");

            this.WriteStatusModelToRedis();
        }


        public StatusModel(string userMessage, string stackTrace)
        {
            this.UserMessage = userMessage;
            this.Success = false;
            this.StatckTrace = stackTrace;
        }
    }
}
