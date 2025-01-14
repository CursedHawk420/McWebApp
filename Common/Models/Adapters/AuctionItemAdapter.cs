using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using J = Newtonsoft.Json.JsonPropertyAttribute;
using N = Newtonsoft.Json.NullValueHandling;

namespace Highgeek.McWebApp.Common.Models.Adapters.Auction
{

    public partial class AuctionItemAdapter
    {
        [J("Owner", NullValueHandling = N.Ignore)] public string Owner { get; set; }
        [J("GameItem", NullValueHandling = N.Ignore)] public string GameItem { get; set; }
        [J("Price", NullValueHandling = N.Ignore)] public long? Price { get; set; }
        [J("Datetime", NullValueHandling = N.Ignore)] public string Datetime { get; set; }
    }

    public partial class AuctionItemAdapter
    {
        public static AuctionItemAdapter FromJson(string json) => JsonConvert.DeserializeObject<AuctionItemAdapter>(json, Highgeek.McWebApp.Common.Models.Adapters.Auction.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this AuctionItemAdapter self) => JsonConvert.SerializeObject(self, Highgeek.McWebApp.Common.Models.Adapters.Auction.Converter.Settings);
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
