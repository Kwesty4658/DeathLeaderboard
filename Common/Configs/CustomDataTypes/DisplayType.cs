using Terraria.ModLoader.Config;

namespace DeathLeaderboard.Common.Configs.CustomDataTypes;

public enum DisplayType
{
	[LabelKey("$Mods.DeathLeaderboard.Configs.Common.DisplayTypes.Deaths.Label")]
	[TooltipKey("$Mods.DeathLeaderboard.Configs.Common.DisplayTypes.Deaths.Tooltip")]
	Deaths,

	[LabelKey("$Mods.DeathLeaderboard.Configs.Common.DisplayTypes.DeathsCauses.Label")]
	[TooltipKey("$Mods.DeathLeaderboard.Configs.Common.DisplayTypes.DeathsCauses.Tooltip")]
	DeathsCauses
}