using Terraria.ModLoader.Config;

namespace DeathLeaderboard.Common.Configs;

public class RecursionConfig : ModConfig
{
	public override ConfigScope Mode => ConfigScope.ServerSide;

	[LabelKey("$Mods.DeathLeaderboard.Configs.Common.RecursionDepth.Label")]
	[TooltipKey("$Mods.DeathLeaderboard.Configs.Common.RecursionDepth.Tooltip")]
	public int RecursionDepth = 10;
}