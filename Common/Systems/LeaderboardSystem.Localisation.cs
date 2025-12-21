using DeathLeaderboard.Common.GlobalNPCs;
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
        Header,
        PlayerDeaths,
        DeathsTo,
        NoCauses,
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
        Header          = Mod.GetLocalization("Leaderboard." + nameof(Header));
        PlayerDeaths    = Mod.GetLocalization("Leaderboard." + nameof(PlayerDeaths));
        DeathsTo        = Mod.GetLocalization("Leaderboard." + nameof(DeathsTo));
        NoCauses        = Mod.GetLocalization("Leaderboard." + nameof(NoCauses));
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

    private static string GetLocalisationKey(PlayerDeathReason damageSource)
    {
        if (damageSource.SourceOtherIndex != -1)
        {
            return GetEnvironmentalCause(damageSource).Key;
        }
            
        if (damageSource.SourcePlayerIndex != -1)
        {
            if (Main.player[damageSource.SourcePlayerIndex]?.active == true) 
                return Main.player[damageSource.SourcePlayerIndex].name;
        }
        
        if (damageSource.SourceNPCIndex != -1)
        {
            if (NPCOwner.Owners.TryGetValue(Main.npc[damageSource.SourceNPCIndex].netID, out int netID))
                return Lang.GetNPCName(netID).Key;

            return Lang.GetNPCName(Main.npc[damageSource.SourceNPCIndex].netID).Key;
        }

        if (damageSource.SourceProjectileType != -1)
        {
            if (ProjectileOwner.Owners.TryGetValue(damageSource.SourceProjectileType, out int netID))
                return Lang.GetNPCName(netID).Key;

            return Lang.GetNPCName(damageSource.SourceProjectileType).Key;
        }

        return Unknown.Key;
    }

    private static LocalizedText GetEnvironmentalCause(PlayerDeathReason damageSource)
    {
        return (EnvironmentalCause)damageSource.SourceOtherIndex switch
        {
            EnvironmentalCause.Fell            => Fell,
            EnvironmentalCause.Drowned         => Drowned,
            EnvironmentalCause.Lava            => Lava,
            EnvironmentalCause.Unknown         => Unknown,
            EnvironmentalCause.Slain           => Slain,
            EnvironmentalCause.Slain2          => Slain2,
            EnvironmentalCause.Petrified       => Petrified,
            EnvironmentalCause.Stabbed         => Stabbed,
            EnvironmentalCause.Suffocated      => Suffocated,
            EnvironmentalCause.Burned          => Burned,
            EnvironmentalCause.Poisoned        => Poisoned,
            EnvironmentalCause.Electrocuted    => Electrocuted,
            EnvironmentalCause.AttemptedEscape => AttemptedEscape,
            EnvironmentalCause.Licked          => Licked,
            EnvironmentalCause.Teleport1       => Teleport1,
            EnvironmentalCause.Teleport2Male   => Teleport2Male,
            EnvironmentalCause.Teleport2Female => Teleport2Female,
            EnvironmentalCause.Inferno         => Inferno,
            EnvironmentalCause.DiedInTheDark   => DiedInTheDark,
            EnvironmentalCause.Starved         => Starved,
            EnvironmentalCause.Space           => Space,
            EnvironmentalCause.Empty           => Empty,
            _                                  => Unknown
        };
    }

    private enum EnvironmentalCause
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