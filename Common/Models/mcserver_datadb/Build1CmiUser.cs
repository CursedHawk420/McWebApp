using System;
using System.Collections.Generic;

namespace Highgeek.McWebApp.Common.Models.mcserver_datadb;

public partial class Build1CmiUser
{
    public int Id { get; set; }

    public string? PlayerUuid { get; set; }

    public string? Username { get; set; }

    public string? Nickname { get; set; }

    public string? LogOutLocation { get; set; }

    public string? DeathLocation { get; set; }

    public string? TeleportLocation { get; set; }

    public string? Homes { get; set; }

    public long? LastLoginTime { get; set; }

    public long? LastLogoffTime { get; set; }

    public long? TotalPlayTime { get; set; }

    public long? TFly { get; set; }

    public long? TGod { get; set; }

    public string? Glow { get; set; }

    public string? Ips { get; set; }

    public bool? Cuffed { get; set; }

    public long? AlertUntil { get; set; }

    public string? AlertReason { get; set; }

    public bool? JoinedCounter { get; set; }

    public string? LockedIps { get; set; }

    public long? PTime { get; set; }

    public string? Kits { get; set; }

    public string? Charges { get; set; }

    public string? Cooldowns { get; set; }

    public double? Balance { get; set; }

    public string? Notes { get; set; }

    public string? Rank { get; set; }

    public long? BannedUntil { get; set; }

    public long? BannedAt { get; set; }

    public string? BannedBy { get; set; }

    public string? BanReason { get; set; }

    public string? Ignores { get; set; }

    public string? Vanish { get; set; }

    public string? Economy { get; set; }

    public string? Mail { get; set; }

    public double? FlightCharge { get; set; }

    public string? UserMeta { get; set; }

    public bool? Flying { get; set; }

    public int? Votifier { get; set; }

    public string? Jail { get; set; }

    public long? JailedUntil { get; set; }

    public bool? FakeAccount { get; set; }

    public long? PlaytimeOptimized { get; set; }

    public bool? FlightChargeEnabled { get; set; }

    public string? JailReason { get; set; }

    public string? Skin { get; set; }

    public bool? Collision { get; set; }

    public string? NamePrefix { get; set; }

    public string? NameSuffix { get; set; }

    public string? Warnings { get; set; }

    public string? NameColor { get; set; }

    public string? Muted { get; set; }

    public string? Afrecharge { get; set; }

    public string? DisplayName { get; set; }

    public string? Options { get; set; }

    public string? ChatColor { get; set; }
}
