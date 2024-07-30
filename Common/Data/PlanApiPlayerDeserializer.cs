﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using MCWebApplication1.Areas.Minecraft.Logic.PlanApiPlayer;
//
//    var PlanApiPlayer = PlanApiPlayer.FromJson(jsonString);

namespace Highgeek.McWebApp.Common.Data.Plan
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class PlanApiPlayerDeserializer
    {
        [JsonProperty("kill_data", NullValueHandling = NullValueHandling.Ignore)]
        public KillData KillData { get; set; }

        [JsonProperty("sessions", NullValueHandling = NullValueHandling.Ignore)]
        public List<Session> Sessions { get; set; }

        [JsonProperty("calendar_series", NullValueHandling = NullValueHandling.Ignore)]
        public List<CalendarSery> CalendarSeries { get; set; }

        [JsonProperty("player_deaths", NullValueHandling = NullValueHandling.Ignore)]
        public List<Player> PlayerDeaths { get; set; }

        [JsonProperty("gm_series", NullValueHandling = NullValueHandling.Ignore)]
        public List<GmSery> GmSeries { get; set; }

        [JsonProperty("world_pie_series", NullValueHandling = NullValueHandling.Ignore)]
        public List<WorldSery> WorldPieSeries { get; set; }

        [JsonProperty("server_pie_colors", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ServerPieColors { get; set; }

        [JsonProperty("ping_graph", NullValueHandling = NullValueHandling.Ignore)]
        public PingGraph PingGraph { get; set; }

        [JsonProperty("extensions", NullValueHandling = NullValueHandling.Ignore)]
        public List<Extension> Extensions { get; set; }

        [JsonProperty("servers", NullValueHandling = NullValueHandling.Ignore)]
        public List<Server> Servers { get; set; }

        [JsonProperty("server_pie_series", NullValueHandling = NullValueHandling.Ignore)]
        public List<ServerPieSery> ServerPieSeries { get; set; }

        [JsonProperty("online_activity", NullValueHandling = NullValueHandling.Ignore)]
        public OnlineActivity OnlineActivity { get; set; }

        [JsonProperty("punchcard_series", NullValueHandling = NullValueHandling.Ignore)]
        public List<PunchcardSery> PunchcardSeries { get; set; }

        [JsonProperty("sessions_per_page", NullValueHandling = NullValueHandling.Ignore)]
        public double? SessionsPerPage { get; set; }

        [JsonProperty("player_kills", NullValueHandling = NullValueHandling.Ignore)]
        public List<Player> PlayerKills { get; set; }

        [JsonProperty("nicknames", NullValueHandling = NullValueHandling.Ignore)]
        public List<Nickname> Nicknames { get; set; }

        [JsonProperty("timestamp_f", NullValueHandling = NullValueHandling.Ignore)]
        public string TimestampF { get; set; }

        [JsonProperty("connections", NullValueHandling = NullValueHandling.Ignore)]
        public List<Connection> Connections { get; set; }

        [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore)]
        public double? Timestamp { get; set; }

        [JsonProperty("info", NullValueHandling = NullValueHandling.Ignore)]
        public Info Info { get; set; }

        [JsonProperty("first_day", NullValueHandling = NullValueHandling.Ignore)]
        public double? FirstDay { get; set; }
    }

    public partial class CalendarSery
    {
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("start", NullValueHandling = NullValueHandling.Ignore)]
        public Start? Start { get; set; }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }

        [JsonProperty("end", NullValueHandling = NullValueHandling.Ignore)]
        public double? End { get; set; }
    }

    public partial class Connection
    {
        [JsonProperty("geolocation", NullValueHandling = NullValueHandling.Ignore)]
        public string Geolocation { get; set; }

        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }
    }

    public partial class Extension
    {
        [JsonProperty("playerUUID", NullValueHandling = NullValueHandling.Ignore)]
        public string PlayerUuid { get; set; }

        [JsonProperty("serverUUID", NullValueHandling = NullValueHandling.Ignore)]
        public string ServerUuid { get; set; }

        [JsonProperty("serverName", NullValueHandling = NullValueHandling.Ignore)]
        public string ServerName { get; set; }

        [JsonProperty("extensionData", NullValueHandling = NullValueHandling.Ignore)]
        public List<ExtensionDatum> ExtensionData { get; set; }
    }

    public partial class ExtensionDatum
    {
        [JsonProperty("extensionInformation", NullValueHandling = NullValueHandling.Ignore)]
        public ExtensionInformation ExtensionInformation { get; set; }

        [JsonProperty("tabs", NullValueHandling = NullValueHandling.Ignore)]
        public List<Tab> Tabs { get; set; }

        [JsonProperty("onlyGenericTab", NullValueHandling = NullValueHandling.Ignore)]
        public bool? OnlyGenericTab { get; set; }

        [JsonProperty("wide", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Wide { get; set; }
    }

    public partial class ExtensionInformation
    {
        [JsonProperty("pluginName", NullValueHandling = NullValueHandling.Ignore)]
        public string PluginName { get; set; }

        [JsonProperty("icon", NullValueHandling = NullValueHandling.Ignore)]
        public Icon Icon { get; set; }
    }

    public partial class Icon
    {
        [JsonProperty("family", NullValueHandling = NullValueHandling.Ignore)]
        public string Family { get; set; }

        [JsonProperty("familyClass", NullValueHandling = NullValueHandling.Ignore)]
        public string FamilyClass { get; set; }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }

        [JsonProperty("colorClass", NullValueHandling = NullValueHandling.Ignore)]
        public string ColorClass { get; set; }

        [JsonProperty("iconName", NullValueHandling = NullValueHandling.Ignore)]
        public string IconName { get; set; }
    }

    public partial class Tab
    {
        [JsonProperty("tabInformation", NullValueHandling = NullValueHandling.Ignore)]
        public TabInformation TabInformation { get; set; }

        [JsonProperty("values", NullValueHandling = NullValueHandling.Ignore)]
        public List<Value> Values { get; set; }

        [JsonProperty("tableData", NullValueHandling = NullValueHandling.Ignore)]
        public List<TableDatum> TableData { get; set; }
    }

    public partial class TabInformation
    {
        [JsonProperty("tabName", NullValueHandling = NullValueHandling.Ignore)]
        public string TabName { get; set; }

        [JsonProperty("icon", NullValueHandling = NullValueHandling.Ignore)]
        public Icon Icon { get; set; }

        [JsonProperty("elementOrder", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ElementOrder { get; set; }

        [JsonProperty("tabPriority", NullValueHandling = NullValueHandling.Ignore)]
        public double? TabPriority { get; set; }
    }

    public partial class TableDatum
    {
        [JsonProperty("tableName", NullValueHandling = NullValueHandling.Ignore)]
        public string TableName { get; set; }

        [JsonProperty("table", NullValueHandling = NullValueHandling.Ignore)]
        public Table Table { get; set; }

        [JsonProperty("tableColor", NullValueHandling = NullValueHandling.Ignore)]
        public string TableColor { get; set; }

        [JsonProperty("tableColorClass", NullValueHandling = NullValueHandling.Ignore)]
        public string TableColorClass { get; set; }

        [JsonProperty("wide", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Wide { get; set; }
    }

    public partial class Table
    {
        [JsonProperty("columns", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Columns { get; set; }

        [JsonProperty("icons", NullValueHandling = NullValueHandling.Ignore)]
        public List<Icon> Icons { get; set; }

        [JsonProperty("rows", NullValueHandling = NullValueHandling.Ignore)]
        public List<List<Row>> Rows { get; set; }
    }

    public partial class Row
    {
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        [JsonProperty("valueUnformatted", NullValueHandling = NullValueHandling.Ignore)]
        public string ValueUnformatted { get; set; }
    }

    public partial class Value
    {
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public Description Description { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string ValueValue { get; set; }
    }

    public partial class Description
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string DescriptionDescription { get; set; }

        [JsonProperty("icon", NullValueHandling = NullValueHandling.Ignore)]
        public Icon Icon { get; set; }

        [JsonProperty("priority", NullValueHandling = NullValueHandling.Ignore)]
        public double? Priority { get; set; }
    }

    public partial class GmSery
    {
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public List<List<Datum>> Data { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
    }

    public partial class Info
    {
        [JsonProperty("kick_count", NullValueHandling = NullValueHandling.Ignore)]
        public double? KickCount { get; set; }

        [JsonProperty("last_seen", NullValueHandling = NullValueHandling.Ignore)]
        public string LastSeen { get; set; }

        [JsonProperty("mob_kill_count", NullValueHandling = NullValueHandling.Ignore)]
        public double? MobKillCount { get; set; }

        [JsonProperty("registered", NullValueHandling = NullValueHandling.Ignore)]
        public string Registered { get; set; }

        [JsonProperty("activity_index_group", NullValueHandling = NullValueHandling.Ignore)]
        public string ActivityIndexGroup { get; set; }

        [JsonProperty("playtime", NullValueHandling = NullValueHandling.Ignore)]
        public string Playtime { get; set; }

        [JsonProperty("session_median", NullValueHandling = NullValueHandling.Ignore)]
        public string SessionMedian { get; set; }

        [JsonProperty("uuid", NullValueHandling = NullValueHandling.Ignore)]
        public string Uuid { get; set; }

        [JsonProperty("operator", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Operator { get; set; }

        [JsonProperty("afk_time", NullValueHandling = NullValueHandling.Ignore)]
        public string AfkTime { get; set; }

        [JsonProperty("death_count", NullValueHandling = NullValueHandling.Ignore)]
        public double? DeathCount { get; set; }

        [JsonProperty("latest_join_address", NullValueHandling = NullValueHandling.Ignore)]
        public string LatestJoinAddress { get; set; }

        [JsonProperty("worst_ping", NullValueHandling = NullValueHandling.Ignore)]
        public string WorstPing { get; set; }

        [JsonProperty("doubleest_session_length", NullValueHandling = NullValueHandling.Ignore)]
        public string doubleestSessionLength { get; set; }

        [JsonProperty("banned", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Banned { get; set; }

        [JsonProperty("last_seen_raw_value", NullValueHandling = NullValueHandling.Ignore)]
        public double? LastSeenRawValue { get; set; }

        [JsonProperty("favorite_server", NullValueHandling = NullValueHandling.Ignore)]
        public string FavoriteServer { get; set; }

        [JsonProperty("player_kill_count", NullValueHandling = NullValueHandling.Ignore)]
        public double? PlayerKillCount { get; set; }

        [JsonProperty("active_playtime", NullValueHandling = NullValueHandling.Ignore)]
        public string ActivePlaytime { get; set; }

        [JsonProperty("session_count", NullValueHandling = NullValueHandling.Ignore)]
        public double? SessionCount { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("online", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Online { get; set; }

        [JsonProperty("best_ping", NullValueHandling = NullValueHandling.Ignore)]
        public string BestPing { get; set; }

        [JsonProperty("activity_index", NullValueHandling = NullValueHandling.Ignore)]
        public string ActivityIndex { get; set; }

        [JsonProperty("average_ping", NullValueHandling = NullValueHandling.Ignore)]
        public string AveragePing { get; set; }
    }

    public partial class KillData
    {
        [JsonProperty("deaths_7d", NullValueHandling = NullValueHandling.Ignore)]
        public double? Deaths7D { get; set; }

        [JsonProperty("mob_deaths_7d", NullValueHandling = NullValueHandling.Ignore)]
        public double? MobDeaths7D { get; set; }

        [JsonProperty("mob_kills_total", NullValueHandling = NullValueHandling.Ignore)]
        public double? MobKillsTotal { get; set; }

        [JsonProperty("player_kdr_30d", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringConverter))]
        public double? PlayerKdr30D { get; set; }

        [JsonProperty("player_kills_7d", NullValueHandling = NullValueHandling.Ignore)]
        public double? PlayerKills7D { get; set; }

        [JsonProperty("player_deaths_total", NullValueHandling = NullValueHandling.Ignore)]
        public double? PlayerDeathsTotal { get; set; }

        [JsonProperty("deaths_30d", NullValueHandling = NullValueHandling.Ignore)]
        public double? Deaths30D { get; set; }

        [JsonProperty("deaths_total", NullValueHandling = NullValueHandling.Ignore)]
        public double? DeathsTotal { get; set; }

        [JsonProperty("player_kills_total", NullValueHandling = NullValueHandling.Ignore)]
        public double? PlayerKillsTotal { get; set; }

        [JsonProperty("mob_deaths_30d", NullValueHandling = NullValueHandling.Ignore)]
        public double? MobDeaths30D { get; set; }

        [JsonProperty("player_deaths_30d", NullValueHandling = NullValueHandling.Ignore)]
        public double? PlayerDeaths30D { get; set; }

        [JsonProperty("mob_kdr_30d", NullValueHandling = NullValueHandling.Ignore)]
        public string MobKdr30D { get; set; }

        [JsonProperty("player_kdr_total", NullValueHandling = NullValueHandling.Ignore)]
        public string PlayerKdrTotal { get; set; }

        [JsonProperty("weapon_3rd", NullValueHandling = NullValueHandling.Ignore)]
        public string Weapon3Rd { get; set; }

        [JsonProperty("mob_kdr_7d", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringConverter))]
        public double? MobKdr7D { get; set; }

        [JsonProperty("player_kdr_7d", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringConverter))]
        public double? PlayerKdr7D { get; set; }

        [JsonProperty("player_kills_30d", NullValueHandling = NullValueHandling.Ignore)]
        public double? PlayerKills30D { get; set; }

        [JsonProperty("weapon_2nd", NullValueHandling = NullValueHandling.Ignore)]
        public string Weapon2Nd { get; set; }

        [JsonProperty("player_deaths_7d", NullValueHandling = NullValueHandling.Ignore)]
        public double? PlayerDeaths7D { get; set; }

        [JsonProperty("mob_deaths_total", NullValueHandling = NullValueHandling.Ignore)]
        public double? MobDeathsTotal { get; set; }

        [JsonProperty("mob_kills_7d", NullValueHandling = NullValueHandling.Ignore)]
        public double? MobKills7D { get; set; }

        [JsonProperty("mob_kdr_total", NullValueHandling = NullValueHandling.Ignore)]
        public string MobKdrTotal { get; set; }

        [JsonProperty("weapon_1st", NullValueHandling = NullValueHandling.Ignore)]
        public string Weapon1St { get; set; }

        [JsonProperty("mob_kills_30d", NullValueHandling = NullValueHandling.Ignore)]
        public double? MobKills30D { get; set; }
    }

    public partial class Nickname
    {
        [JsonProperty("nickname", NullValueHandling = NullValueHandling.Ignore)]
        public string NicknameNickname { get; set; }

        [JsonProperty("server", NullValueHandling = NullValueHandling.Ignore)]
        public string Server { get; set; }

        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }
    }

    public partial class OnlineActivity
    {
        [JsonProperty("session_count_30d", NullValueHandling = NullValueHandling.Ignore)]
        public double? SessionCount30D { get; set; }

        [JsonProperty("session_count_7d", NullValueHandling = NullValueHandling.Ignore)]
        public double? SessionCount7D { get; set; }

        [JsonProperty("afk_time_7d", NullValueHandling = NullValueHandling.Ignore)]
        public string AfkTime7D { get; set; }

        [JsonProperty("mob_kill_count_30d", NullValueHandling = NullValueHandling.Ignore)]
        public double? MobKillCount30D { get; set; }

        [JsonProperty("active_playtime_7d", NullValueHandling = NullValueHandling.Ignore)]
        public string ActivePlaytime7D { get; set; }

        [JsonProperty("mob_kill_count_7d", NullValueHandling = NullValueHandling.Ignore)]
        public double? MobKillCount7D { get; set; }

        [JsonProperty("afk_time_30d", NullValueHandling = NullValueHandling.Ignore)]
        public string AfkTime30D { get; set; }

        [JsonProperty("average_session_length_30d", NullValueHandling = NullValueHandling.Ignore)]
        public string AverageSessionLength30D { get; set; }

        [JsonProperty("death_count_30d", NullValueHandling = NullValueHandling.Ignore)]
        public double? DeathCount30D { get; set; }

        [JsonProperty("active_playtime_30d", NullValueHandling = NullValueHandling.Ignore)]
        public string ActivePlaytime30D { get; set; }

        [JsonProperty("average_session_length_7d", NullValueHandling = NullValueHandling.Ignore)]
        public string AverageSessionLength7D { get; set; }

        [JsonProperty("median_session_length_7d", NullValueHandling = NullValueHandling.Ignore)]
        public string MedianSessionLength7D { get; set; }

        [JsonProperty("median_session_length_30d", NullValueHandling = NullValueHandling.Ignore)]
        public string MedianSessionLength30D { get; set; }

        [JsonProperty("playtime_30d", NullValueHandling = NullValueHandling.Ignore)]
        public string Playtime30D { get; set; }

        [JsonProperty("player_kill_count_30d", NullValueHandling = NullValueHandling.Ignore)]
        public double? PlayerKillCount30D { get; set; }

        [JsonProperty("death_count_7d", NullValueHandling = NullValueHandling.Ignore)]
        public double? DeathCount7D { get; set; }

        [JsonProperty("playtime_7d", NullValueHandling = NullValueHandling.Ignore)]
        public string Playtime7D { get; set; }

        [JsonProperty("player_kill_count_7d", NullValueHandling = NullValueHandling.Ignore)]
        public double? PlayerKillCount7D { get; set; }
    }

    public partial class PingGraph
    {
        [JsonProperty("max_ping_series", NullValueHandling = NullValueHandling.Ignore)]
        public List<PingSery> MaxPingSeries { get; set; }

        [JsonProperty("avg_ping_series", NullValueHandling = NullValueHandling.Ignore)]
        public List<PingSery> AvgPingSeries { get; set; }

        [JsonProperty("min_ping_series", NullValueHandling = NullValueHandling.Ignore)]
        public List<PingSery> MinPingSeries { get; set; }

        [JsonProperty("colors", NullValueHandling = NullValueHandling.Ignore)]
        public Colors Colors { get; set; }
    }

    public partial class PingSery
    {
        [JsonProperty("x", NullValueHandling = NullValueHandling.Ignore)]
        public double? X { get; set; }

        [JsonProperty("y", NullValueHandling = NullValueHandling.Ignore)]
        public double? Y { get; set; }
    }

    public partial class Colors
    {
        [JsonProperty("min", NullValueHandling = NullValueHandling.Ignore)]
        public string Min { get; set; }

        [JsonProperty("avg", NullValueHandling = NullValueHandling.Ignore)]
        public string Avg { get; set; }

        [JsonProperty("max", NullValueHandling = NullValueHandling.Ignore)]
        public string Max { get; set; }
    }

    public partial class Player
    {
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        [JsonProperty("weapon", NullValueHandling = NullValueHandling.Ignore)]
        public string Weapon { get; set; }

        [JsonProperty("timeSinceRegisterMillis", NullValueHandling = NullValueHandling.Ignore)]
        public double? TimeSinceRegisterMillis { get; set; }

        [JsonProperty("victimUUID", NullValueHandling = NullValueHandling.Ignore)]
        public string VictimUuid { get; set; }

        [JsonProperty("serverUUID", NullValueHandling = NullValueHandling.Ignore)]
        public string ServerUuid { get; set; }

        [JsonProperty("victimName", NullValueHandling = NullValueHandling.Ignore)]
        public string VictimName { get; set; }

        [JsonProperty("timeSinceRegisterFormatted", NullValueHandling = NullValueHandling.Ignore)]
        public string TimeSinceRegisterFormatted { get; set; }

        [JsonProperty("serverName", NullValueHandling = NullValueHandling.Ignore)]
        public string ServerName { get; set; }

        [JsonProperty("victim", NullValueHandling = NullValueHandling.Ignore)]
        public string Victim { get; set; }

        [JsonProperty("killer", NullValueHandling = NullValueHandling.Ignore)]
        public string Killer { get; set; }

        [JsonProperty("killerUUID", NullValueHandling = NullValueHandling.Ignore)]
        public string KillerUuid { get; set; }

        [JsonProperty("killerName", NullValueHandling = NullValueHandling.Ignore)]
        public string KillerName { get; set; }
    }

    public partial class PunchcardSery
    {
        [JsonProperty("x", NullValueHandling = NullValueHandling.Ignore)]
        public double? X { get; set; }

        [JsonProperty("y", NullValueHandling = NullValueHandling.Ignore)]
        public double? Y { get; set; }

        [JsonProperty("z", NullValueHandling = NullValueHandling.Ignore)]
        public double? Z { get; set; }

        [JsonProperty("marker", NullValueHandling = NullValueHandling.Ignore)]
        public Marker Marker { get; set; }
    }

    public partial class Marker
    {
        [JsonProperty("radius", NullValueHandling = NullValueHandling.Ignore)]
        public double? Radius { get; set; }
    }

    public partial class ServerPieSery
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("y", NullValueHandling = NullValueHandling.Ignore)]
        public double? Y { get; set; }
    }

    public partial class Server
    {
        [JsonProperty("server_name", NullValueHandling = NullValueHandling.Ignore)]
        public string ServerName { get; set; }

        [JsonProperty("last_seen", NullValueHandling = NullValueHandling.Ignore)]
        public string LastSeen { get; set; }

        [JsonProperty("registered", NullValueHandling = NullValueHandling.Ignore)]
        public string Registered { get; set; }

        [JsonProperty("gm_series", NullValueHandling = NullValueHandling.Ignore)]
        public List<GmSery> GmSeries { get; set; }

        [JsonProperty("playtime", NullValueHandling = NullValueHandling.Ignore)]
        public string Playtime { get; set; }

        [JsonProperty("session_median", NullValueHandling = NullValueHandling.Ignore)]
        public string SessionMedian { get; set; }

        [JsonProperty("world_pie_series", NullValueHandling = NullValueHandling.Ignore)]
        public List<WorldSery> WorldPieSeries { get; set; }

        [JsonProperty("operator", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Operator { get; set; }

        [JsonProperty("mob_kills", NullValueHandling = NullValueHandling.Ignore)]
        public double? MobKills { get; set; }

        [JsonProperty("afk_time", NullValueHandling = NullValueHandling.Ignore)]
        public string AfkTime { get; set; }

        [JsonProperty("session_count", NullValueHandling = NullValueHandling.Ignore)]
        public double? SessionCount { get; set; }

        [JsonProperty("join_address", NullValueHandling = NullValueHandling.Ignore)]
        public string JoinAddress { get; set; }

        [JsonProperty("player_kills", NullValueHandling = NullValueHandling.Ignore)]
        public double? PlayerKills { get; set; }

        [JsonProperty("doubleest_session_length", NullValueHandling = NullValueHandling.Ignore)]
        public string doubleestSessionLength { get; set; }

        [JsonProperty("banned", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Banned { get; set; }

        [JsonProperty("server_uuid", NullValueHandling = NullValueHandling.Ignore)]
        public string ServerUuid { get; set; }

        [JsonProperty("deaths", NullValueHandling = NullValueHandling.Ignore)]
        public double? Deaths { get; set; }
    }

    public partial class WorldSery
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("y", NullValueHandling = NullValueHandling.Ignore)]
        public double? Y { get; set; }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }

        [JsonProperty("drilldown", NullValueHandling = NullValueHandling.Ignore)]
        public string Drilldown { get; set; }
    }

    public partial class Session
    {
        [JsonProperty("server_name", NullValueHandling = NullValueHandling.Ignore)]
        public string ServerName { get; set; }

        [JsonProperty("start", NullValueHandling = NullValueHandling.Ignore)]
        public string Start { get; set; }

        [JsonProperty("length", NullValueHandling = NullValueHandling.Ignore)]
        public string Length { get; set; }

        [JsonProperty("gm_series", NullValueHandling = NullValueHandling.Ignore)]
        public List<GmSery> GmSeries { get; set; }

        [JsonProperty("player_uuid", NullValueHandling = NullValueHandling.Ignore)]
        public string PlayerUuid { get; set; }

        [JsonProperty("most_used_world", NullValueHandling = NullValueHandling.Ignore)]
        public string MostUsedWorld { get; set; }

        [JsonProperty("first_session", NullValueHandling = NullValueHandling.Ignore)]
        public bool? FirstSession { get; set; }

        [JsonProperty("mob_kills", NullValueHandling = NullValueHandling.Ignore)]
        public double? MobKills { get; set; }

        [JsonProperty("player_url_name", NullValueHandling = NullValueHandling.Ignore)]
        public string PlayerUrlName { get; set; }

        [JsonProperty("afk_time", NullValueHandling = NullValueHandling.Ignore)]
        public string AfkTime { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("player_kills", NullValueHandling = NullValueHandling.Ignore)]
        public List<Player> PlayerKills { get; set; }

        [JsonProperty("join_address", NullValueHandling = NullValueHandling.Ignore)]
        public string JoinAddress { get; set; }

        [JsonProperty("end", NullValueHandling = NullValueHandling.Ignore)]
        public string End { get; set; }

        [JsonProperty("player_name", NullValueHandling = NullValueHandling.Ignore)]
        public string PlayerName { get; set; }

        [JsonProperty("world_series", NullValueHandling = NullValueHandling.Ignore)]
        public List<WorldSery> WorldSeries { get; set; }

        [JsonProperty("avg_ping", NullValueHandling = NullValueHandling.Ignore)]
        public string AvgPing { get; set; }

        [JsonProperty("server_url_name", NullValueHandling = NullValueHandling.Ignore)]
        public string ServerUrlName { get; set; }

        [JsonProperty("server_uuid", NullValueHandling = NullValueHandling.Ignore)]
        public string ServerUuid { get; set; }

        [JsonProperty("deaths", NullValueHandling = NullValueHandling.Ignore)]
        public double? Deaths { get; set; }
    }

    public partial struct Start
    {
        public DateTimeOffset? DateTime;
        public double? Integer;

        public static implicit operator Start(DateTimeOffset DateTime) => new Start { DateTime = DateTime };
        public static implicit operator Start(double Integer) => new Start { Integer = Integer };
    }

    public partial struct Datum
    {
        public double? Integer;
        public string String;

        public static implicit operator Datum(double Integer) => new Datum { Integer = Integer };
        public static implicit operator Datum(string String) => new Datum { String = String };
    }

    public partial class PlanApiPlayerDeserializer
    {
        public static PlanApiPlayerDeserializer FromJson(string json) => JsonConvert.DeserializeObject<PlanApiPlayerDeserializer>(json, Highgeek.McWebApp.Common.Data.Plan.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this PlanApiPlayerDeserializer self) => JsonConvert.SerializeObject(self, Highgeek.McWebApp.Common.Data.Plan.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                StartConverter.Singleton,
                DatumConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class StartConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Start) || t == typeof(Start?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    var integerValue = serializer.Deserialize<double>(reader);
                    return new Start { Integer = integerValue };
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    DateTimeOffset dt;
                    if (DateTimeOffset.TryParse(stringValue, out dt))
                    {
                        return new Start { DateTime = dt };
                    }
                    break;
            }
            throw new Exception("Cannot unmarshal type Start");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (Start)untypedValue;
            if (value.Integer != null)
            {
                serializer.Serialize(writer, value.Integer.Value);
                return;
            }
            if (value.DateTime != null)
            {
                serializer.Serialize(writer, value.DateTime.Value.ToString("o", System.Globalization.CultureInfo.InvariantCulture));
                return;
            }
            throw new Exception("Cannot marshal type Start");
        }

        public static readonly StartConverter Singleton = new StartConverter();
    }

    internal class DatumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Datum) || t == typeof(Datum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    var integerValue = serializer.Deserialize<double>(reader);
                    return new Datum { Integer = integerValue };
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    return new Datum { String = stringValue };
            }
            throw new Exception("Cannot unmarshal type Datum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (Datum)untypedValue;
            if (value.Integer != null)
            {
                serializer.Serialize(writer, value.Integer.Value);
                return;
            }
            if (value.String != null)
            {
                serializer.Serialize(writer, value.String);
                return;
            }
            throw new Exception("Cannot marshal type Datum");
        }

        public static readonly DatumConverter Singleton = new DatumConverter();
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(double) || t == typeof(double?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            double l;
            if (double.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type double'\" & "+value+" & \"'\"");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (double)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}