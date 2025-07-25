using Terraria.ModLoader.Config;

namespace DeathLeaderboard.Common.Configs;

public class LeaderboardFormattingConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ServerSide;

    [DrawTicks]
    [LabelKey("$Mods.DeathLeaderboard.Configs.Common.DisplayTypes.Label")]
    [TooltipKey("$Mods.DeathLeaderboard.Configs.Common.DisplayTypes.Tooltip")]
    public DisplayType DisplayTypes;

    public enum DisplayType
    {
        [LabelKey("$Mods.DeathLeaderboard.Configs.Common.DisplayTypes.Deaths.Label")]
        [TooltipKey("$Mods.DeathLeaderboard.Configs.Common.DisplayTypes.Deaths.Tooltip")]
        Deaths,

        [LabelKey("$Mods.DeathLeaderboard.Configs.Common.DisplayTypes.DeathsCauses.Label")]
        [TooltipKey("$Mods.DeathLeaderboard.Configs.Common.DisplayTypes.DeathsCauses.Tooltip")]
        DeathsCauses
    }
}