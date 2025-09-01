using DeathLeaderboard.Common.GlobalProjectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DeathLeaderboard.Common.Systems;

internal sealed partial class LeaderboardSystem : ModSystem
{
    #region LocalisedText

    private static LocalizedText
        s_header,
        s_playerDeaths,
        s_deathsTo,
        s_noCauses,
        Fell,
        Drowned,
        Lava,
        Slain,
        Slain2,
        Petrified,
        Stabbed,
        Suffocated,
        Burned,
        Poisoned,
        Electrocuted,
        AttemptedEscape,
        Licked,
        Teleport1,
        Teleport2Male,
        Teleport2Female,
        Inferno,
        DiedInTheDark,
        Starved,
        Space,
        Unknown,
        Empty;

    #endregion LocalisedText

    public override void OnLocalizationsLoaded()
    {
        s_header        = Mod.GetLocalization("Leaderboard.Header");
        s_playerDeaths  = Mod.GetLocalization("Leaderboard.PlayerDeaths");
        s_deathsTo      = Mod.GetLocalization("Leaderboard.DeathsTo");
        s_noCauses      = Mod.GetLocalization("Leaderboard.NoCauses");
        Fell            = Mod.GetLocalization("Leaderboard." + nameof(Fell));
        Drowned         = Mod.GetLocalization("Leaderboard." + nameof(Drowned));
        Lava            = Mod.GetLocalization("Leaderboard." + nameof(Lava));
        Slain           = Mod.GetLocalization("Leaderboard." + nameof(Slain));
        Slain2          = Mod.GetLocalization("Leaderboard." + nameof(Slain2));
        Petrified       = Mod.GetLocalization("Leaderboard." + nameof(Petrified));
        Stabbed         = Mod.GetLocalization("Leaderboard." + nameof(Stabbed));
        Suffocated      = Mod.GetLocalization("Leaderboard." + nameof(Suffocated));
        Burned          = Mod.GetLocalization("Leaderboard." + nameof(Burned));
        Poisoned        = Mod.GetLocalization("Leaderboard." + nameof(Poisoned));
        Electrocuted    = Mod.GetLocalization("Leaderboard." + nameof(Electrocuted));
        AttemptedEscape = Mod.GetLocalization("Leaderboard." + nameof(AttemptedEscape));
        Licked          = Mod.GetLocalization("Leaderboard." + nameof(Licked));
        Teleport1       = Mod.GetLocalization("Leaderboard." + nameof(Teleport1));
        Teleport2Male   = Mod.GetLocalization("Leaderboard." + nameof(Teleport2Male));
        Teleport2Female = Mod.GetLocalization("Leaderboard." + nameof(Teleport2Female));
        Inferno         = Mod.GetLocalization("Leaderboard." + nameof(Inferno));
        DiedInTheDark   = Mod.GetLocalization("Leaderboard." + nameof(DiedInTheDark));
        Starved         = Mod.GetLocalization("Leaderboard." + nameof(Starved));
        Space           = Mod.GetLocalization("Leaderboard." + nameof(Space));
        Empty           = Mod.GetLocalization("Leaderboard." + nameof(Empty));
        Unknown         = Mod.GetLocalization("Leaderboard." + nameof(Unknown));
    }

    private static string GetLocalisationKey(string playerName, PlayerDeathReason damageSource)
    {
        if (damageSource.SourceOtherIndex != -1) 
            return GetOtherCause(damageSource).Key;

        if (damageSource.SourcePlayerIndex != -1)
        {
            var player = Main.player[damageSource.SourcePlayerIndex];
            if (player?.active == true) 
                return player.name;
        }

        if (damageSource.SourceProjectileType != -1)
            return Lang.GetNPCName(ProjectileOwner.ProjectileOwners[damageSource.SourceProjectileType]).Key;

        if (damageSource.SourceNPCIndex != -1)
            return damageSource.TryGetCausingEntity(out var entity) 
                ? Lang.GetNPCName(Main.npc[entity.whoAmI].netID).Key
                : Unknown.Key;

        return Unknown.Key;
    }

    private static LocalizedText GetOtherCause(PlayerDeathReason damageSource)
    {
        return (OtherDeathCause)damageSource.SourceOtherIndex switch
        {
            OtherDeathCause.Fell            => Fell,
            OtherDeathCause.Drowned         => Drowned,
            OtherDeathCause.Lava            => Lava,
            OtherDeathCause.Unknown         => Unknown,
            OtherDeathCause.Slain           => Slain,
            OtherDeathCause.Slain2          => Slain2,
            OtherDeathCause.Petrified       => Petrified,
            OtherDeathCause.Stabbed         => Stabbed,
            OtherDeathCause.Suffocated      => Suffocated,
            OtherDeathCause.Burned          => Burned,
            OtherDeathCause.Poisoned        => Poisoned,
            OtherDeathCause.Electrocuted    => Electrocuted,
            OtherDeathCause.AttemptedEscape => AttemptedEscape,
            OtherDeathCause.Licked          => Licked,
            OtherDeathCause.Teleport1       => Teleport1,
            OtherDeathCause.Teleport2Male   => Teleport2Male,
            OtherDeathCause.Teleport2Female => Teleport2Female,
            OtherDeathCause.Inferno         => Inferno,
            OtherDeathCause.DiedInTheDark   => DiedInTheDark,
            OtherDeathCause.Starved         => Starved,
            OtherDeathCause.Space           => Space,
            OtherDeathCause.Empty           => Empty,
            _                               => Unknown
        };
    }

    private enum OtherDeathCause
    {
        Fell            = 0,
        Drowned         = 1,
        Lava            = 2,
        Unknown         = 3,
        Slain           = 4,
        Petrified       = 5,
        Stabbed         = 6,
        Suffocated      = 7,
        Burned          = 8,
        Poisoned        = 9,
        Electrocuted    = 10,
        AttemptedEscape = 11,
        Licked          = 12,
        Teleport1       = 13,
        Teleport2Male   = 14,
        Teleport2Female = 15,
        Inferno         = 16,
        DiedInTheDark   = 17,
        Starved         = 18,
        Space           = 19,
        Empty           = 254,
        Slain2          = 255
    }
}