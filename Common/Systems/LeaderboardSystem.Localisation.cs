using DeathLeaderboard.Common.GlobalNPCs;
using DeathLeaderboard.Common.GlobalProjectiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;

namespace DeathLeaderboard.Common.Systems;

internal partial class LeaderboardSystem : ModSystem
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

    // Adapted from Lang.CreateDeathMessage()
    private static NetworkText GetLocalisationKey(PlayerDeathReason damageSource)
    {
        var text = NetworkText.Empty;
        
        // Death cause is projectile
        if (damageSource.SourceProjectileLocalIndex >= 0)
        {
            /*
             * This is confusing, if you aren't me. Possibly if you are me.
             * Get npc localisation key by:
             * indexing into Main.projectile with damageSource.SourceProjectileLocalIndex
             * using GetGlobalProjectile to get the ProjectileOwner instance attached to this projectile
             * taking ProjectileOwner.Owner to get the netID of the npc that spawned this projectile
             * then finally passing that netID to Lang.GetNPCName
             */
            text = NetworkText.FromKey(
                Lang.GetNPCName(
                    Main.projectile[damageSource.SourceProjectileLocalIndex]
                        .GetGlobalProjectile<ProjectileOwner>()
                            .Owner
                    ).Key
                );
        }

        // Death cause is npc
        if (damageSource.SourceNPCIndex >= 0)
        {
            text = Main.npc[damageSource.SourceNPCIndex].GetGivenOrTypeNetName();
        }
        
        // Death cause is player (and not server???)
        if (damageSource.SourcePlayerIndex is >= 0 and < 255)
        {
            text = NetworkText.FromLiteral(Main.player[damageSource.SourcePlayerIndex].name);
        }
        
        // if killed by a player's item (unsure if this is what the vanilla code means)
        if (damageSource.SourceItem is not null)
        {
            text = NetworkText.FromKey(Lang.GetItemName(damageSource.SourceItem.netID).Key);
        }
        
        // if killed by anything else (environmental)
        if (damageSource.SourceOtherIndex >= 0)
        {
            return damageSource.SourceOtherIndex switch
            {
                0 => NetworkText.FromKey(Fell.Key),
                1 => NetworkText.FromKey(Drowned.Key),
                2 => NetworkText.FromKey(Lava.Key),
                3 => NetworkText.FromKey(Slain.Key),
                4 => NetworkText.FromKey(Slain.Key),
                5 => NetworkText.FromKey(Petrified.Key),
                6 => NetworkText.FromKey(Stabbed.Key),
                7 => NetworkText.FromKey(Suffocated.Key),
                8 => NetworkText.FromKey(Burned.Key),
                9 => NetworkText.FromKey(Poisoned.Key),
                10 => NetworkText.FromKey(Electrocuted.Key),
                11 => NetworkText.FromKey(AttemptedEscape.Key),
                12 => NetworkText.FromKey(Licked.Key),
                13 => NetworkText.FromKey(Teleport1.Key),
                14 => NetworkText.FromKey(Teleport2Male.Key),
                15 => NetworkText.FromKey(Teleport2Female.Key),
                16 => NetworkText.FromKey(Inferno.Key),
                17 => NetworkText.FromKey(DiedInTheDark.Key),
                18 => NetworkText.FromKey(Starved.Key),
                19 => NetworkText.FromKey(Space.Key),
                254 => NetworkText.Empty,
                _ => NetworkText.FromKey(Slain.Key)
            };
        }
        return text;
    }
}