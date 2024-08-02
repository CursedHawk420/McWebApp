using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Highgeek.McWebApp.Common.Models.Minecraft.ServerListModel
{

    public partial class ServerListModel
    {
        [JsonProperty("serverName")]
        public string ServerName { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("playerList")]
        public List<string> PlayerList { get; set; }
    }

    public partial class ServerListModel
    {
        public static ServerListModel FromJson(string json) => JsonConvert.DeserializeObject<ServerListModel>(json, Highgeek.McWebApp.Common.Models.Minecraft.ServerListModel.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this ServerListModel self) => JsonConvert.SerializeObject(self, Highgeek.McWebApp.Common.Models.Minecraft.ServerListModel.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
