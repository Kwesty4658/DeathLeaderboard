using System.Text;

using DeathLeaderboard.Common.GlobalProjectiles;

using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DeathLeaderboard.Common.Systems;

internal sealed class LeaderboardLocalisation : ModSystem
{
	#region LocalisedText
	internal static LocalizedText
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

	private static LocalizedText GetText(string key) =>
		Language.GetText($"Mods.{nameof(DeathLeaderboard)}.DeathCauses.{key}");

	public override void PostSetupContent()
	{
		Fell            = GetText(nameof(Fell));
		Drowned         = GetText(nameof(Drowned));
		Lava            = GetText(nameof(Lava));
		Slain           = GetText(nameof(Slain));
		Slain2          = GetText(nameof(Slain2));
		Petrified       = GetText(nameof(Petrified));
		Stabbed         = GetText(nameof(Stabbed));
		Suffocated      = GetText(nameof(Suffocated));
		Burned          = GetText(nameof(Burned));
		Poisoned        = GetText(nameof(Poisoned));
		Electrocuted    = GetText(nameof(Electrocuted));
		AttemptedEscape = GetText(nameof(AttemptedEscape));
		Licked          = GetText(nameof(Licked));
		Teleport1       = GetText(nameof(Teleport1));
		Teleport2Male   = GetText(nameof(Teleport2Male));
		Teleport2Female = GetText(nameof(Teleport2Female));
		Inferno         = GetText(nameof(Inferno));
		DiedInTheDark   = GetText(nameof(DiedInTheDark));
		Starved         = GetText(nameof(Starved));
		Space           = GetText(nameof(Space));
		Empty           = GetText(nameof(Empty));
		Unknown         = GetText(nameof(Unknown));
	}

	internal static string GetLocalisationKey(string playerName, PlayerDeathReason damageSource)
	{
		if (damageSource.SourceOtherIndex != -1)
			return GetOtherCause(damageSource).Key;

		if (damageSource.SourcePlayerIndex != -1)
			return damageSource.TryGetCausingEntity(out Entity entity)
				? Lang.GetNPCName(Main.npc[entity.whoAmI].netID).Key
				: Unknown.Key;

		if (damageSource.SourceProjectileType != -1 || damageSource.SourceProjectileLocalIndex != -1)
			return Lang.GetNPCName(GetProjectileOwner(damageSource).netID).Key;

		if (damageSource.SourceNPCIndex != -1)
		{
			if (damageSource.TryGetCausingEntity(out Entity entity))
			{
				return Main.npc[entity.whoAmI].ModNPC != null
					? Main.npc[entity.whoAmI].ModNPC.DisplayName.Key
					: Lang.GetNPCName(Main.npc[entity.whoAmI].netID).Key;
			}
		}

		return Unknown.Key;
	}

	private static NPC GetProjectileOwner(PlayerDeathReason damageSource)
	{
		return Main.projectile[damageSource.SourceProjectileType]
		                            .GetGlobalProjectile<ProjectileParentGlobalProjectile>().Parent;
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