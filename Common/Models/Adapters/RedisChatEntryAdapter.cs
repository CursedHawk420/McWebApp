namespace Highgeek.McWebApp.Common.Models.Adapters
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;


    public partial class RedisChatEntryAdapter
    {
        [JsonProperty("uuid", NullValueHandling = NullValueHandling.Ignore)]
        public string Uuid { get; set; }

        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }

        [JsonProperty("nickname", NullValueHandling = NullValueHandling.Ignore)]
        public string Nickname { get; set; }

        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty("primarygroup", NullValueHandling = NullValueHandling.Ignore)]
        public string Primarygroup { get; set; }

        [JsonProperty("datetime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? Datetime { get; set; }

        [JsonProperty("channel", NullValueHandling = NullValueHandling.Ignore)]
        public string Channel { get; set; }

        [JsonProperty("channelprefix", NullValueHandling = NullValueHandling.Ignore)]
        public string Channelprefix { get; set; }

        [JsonProperty("source", NullValueHandling = NullValueHandling.Ignore)]
        public string Source { get; set; }

        [JsonProperty("servername", NullValueHandling = NullValueHandling.Ignore)]
        public string Servername { get; set; }

        [JsonProperty("prefix", NullValueHandling = NullValueHandling.Ignore)]
        public string? Prefix { get; set; }

        [JsonProperty("suffix", NullValueHandling = NullValueHandling.Ignore)]
        public string? Suffix { get; set; }

        [JsonProperty("playeruuid", NullValueHandling = NullValueHandling.Ignore)]
        public string? PlayerUuid { get; set; }

        [JsonProperty("prettyservername", NullValueHandling = NullValueHandling.Ignore)]
        public string? PrettyServerName { get; set; }

    }
    public partial class RedisChatEntryAdapter
    {
        public static RedisChatEntryAdapter FromJson(string json)
        {
            if (json == null) return null;

            return JsonConvert.DeserializeObject<RedisChatEntryAdapter>(json, Highgeek.McWebApp.Common.Models.Adapters.Converter.Settings);
        }
    }

    public static class Serialize
    {
        public static string ToJson(this RedisChatEntryAdapter self) => JsonConvert.SerializeObject(self, Highgeek.McWebApp.Common.Models.Adapters.Converter.Settings);
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
