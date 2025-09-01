using DeathLeaderboard.Common.Configs.CustomDataTypes;
using Terraria.ModLoader.Config;

namespace DeathLeaderboard.Common.Configs;

public class LeaderboardFormattingConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ServerSide;

    [DrawTicks]
    [LabelKey("$Mods.DeathLeaderboard.Configs.Common.DisplayTypes.Label")]
    [TooltipKey("$Mods.DeathLeaderboard.Configs.Common.DisplayTypes.Tooltip")]
    public DisplayType DisplayTypes;
}