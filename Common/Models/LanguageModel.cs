
using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Highgeek.McWebApp.Common.Models.Language
{

    public partial class LanguageModel
    {
        [JsonProperty("LanguageKeys")]
        public List<LanguageKey> LanguageKeys { get; set; }
    }

    public partial class LanguageKey
    {
        [JsonProperty("key")]
        public string? Key { get; set; }

        [JsonProperty("cs")]
        public string? Cs { get; set; }

        [JsonProperty("en")]
        public string? En { get; set; }
    }

    public partial class LanguageModel
    {
        public static LanguageModel FromJson(string json) => JsonConvert.DeserializeObject<LanguageModel>(json, Highgeek.McWebApp.Common.Models.Language.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this LanguageModel self) => JsonConvert.SerializeObject(self, Highgeek.McWebApp.Common.Models.Language.Converter.Settings);
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
