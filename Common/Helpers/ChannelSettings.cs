
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using Highgeek.McWebApp.Common.Helpers;
//
//    var welcome = Welcome.FromJson(jsonString);

namespace Highgeek.McWebApp.Common.Helpers.Channels
{
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ChannelSettingsAdapter
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("fancyName")]
        public string FancyName { get; set; }

        [JsonProperty("prefix")]
        public string Prefix { get; set; }

        [JsonProperty("isLocal")]
        public bool IsLocal { get; set; }

        [JsonProperty("isDefault")]
        public bool IsDefault { get; set; }
    }

    public partial class ChannelSettingsAdapter
    {
        public static ChannelSettingsAdapter FromJson(string json) => JsonConvert.DeserializeObject<ChannelSettingsAdapter>(json, Highgeek.McWebApp.Common.Helpers.Channels.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this ChannelSettingsAdapter self) => JsonConvert.SerializeObject(self, Highgeek.McWebApp.Common.Helpers.Channels.Converter.Settings);
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
