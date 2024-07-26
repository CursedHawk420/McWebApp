
using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Highgeek.McWebApp.Common.Models.Adapters.LuckpermsRedisLogAdapter
{
    public partial class LuckpermsRedisLogAdapter
    {
        [JsonProperty("entry")]
        public string Entry { get; set; }

        [JsonProperty("targetName")]
        public string TargetName { get; set; }

        [JsonProperty("sourceUuid")]
        public Guid SourceUuid { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("logId")]
        public Guid LogId { get; set; }

        [JsonProperty("sourceName")]
        public string SourceName { get; set; }

        [JsonProperty("eventCanonName")]
        public string EventCanonName { get; set; }

        [JsonProperty("targetUuid")]
        public Guid TargetUuid { get; set; }

        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }
    }

    public partial class LuckpermsRedisLogAdapter
    {
        public static LuckpermsRedisLogAdapter FromJson(string json) => JsonConvert.DeserializeObject<LuckpermsRedisLogAdapter>(json, Highgeek.McWebApp.Common.Models.Adapters.LuckpermsRedisLogAdapter.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this LuckpermsRedisLogAdapter self) => JsonConvert.SerializeObject(self, Highgeek.McWebApp.Common.Models.Adapters.LuckpermsRedisLogAdapter.Converter.Settings);
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
