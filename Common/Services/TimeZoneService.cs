using Microsoft.JSInterop;
using NetTopologySuite.Triangulate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highgeek.McWebApp.Common.Services
{
    public class TimeZoneService
    {
        private readonly IJSRuntime _jsRuntime;

        private TimeSpan? _userOffset;

        public TimeZoneService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            Init();
        }

        public async void Init()
        {
            if (_userOffset == null)
            {
                int offsetInMinutes = await _jsRuntime.InvokeAsync<int>("blazorGetTimezoneOffset");
                _userOffset = TimeSpan.FromMinutes(-offsetInMinutes);
            }
        }

        public async ValueTask<DateTimeOffset> GetLocalDateTime(DateTimeOffset dateTime)
        {
            if (_userOffset == null)
            {
                int offsetInMinutes = await _jsRuntime.InvokeAsync<int>("blazorGetTimezoneOffset");
                _userOffset = TimeSpan.FromMinutes(-offsetInMinutes);
            }

            return dateTime.ToOffset(_userOffset.Value);
        }
    }
}
