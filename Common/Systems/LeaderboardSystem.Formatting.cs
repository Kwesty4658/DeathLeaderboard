using System.Collections.Generic;
using System.Linq;
using DeathLeaderboard.Common.Configs;
using DeathLeaderboard.Common.Configs.CustomDataTypes;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DeathLeaderboard.Common.Systems;

internal sealed partial class LeaderboardSystem : ModSystem
{
    private static List<NetworkText> s_lines;
    
    internal static List<NetworkText> LeaderboardWithCauses()
    {
        bool lbType = ModContent.GetInstance<LeaderboardFormattingConfig>().DisplayTypes == DisplayType.Deaths;
        
        s_lines = [s_header.ToNetworkText()];

        foreach (var player in s_leaderboardPlayers.OrderByDescending(p => p.Deaths))
        {
            var mostCommonCause = player.Causes.OrderByDescending(c => c.Value).FirstOrDefault();

            if (mostCommonCause.Key == null)
            {
                s_lines.Add(NetworkText.FromKey(
                    s_playerDeaths.Key, // {player.Name}: {player.Deaths} | {s_noCauses.Key}
                    player.Name,
                    player.Deaths,
                    NetworkText.FromKey(s_noCauses.Key)
                ));
                continue;
            }

            s_lines.Add(NetworkText.FromKey(
                s_playerDeaths.Key, // {player.Name} deaths to {player.Deaths} | {s_deathsTo.Key}
                player.Name,
                player.Deaths,
                NetworkText.FromKey(s_deathsTo.Key, mostCommonCause.Value, NetworkText.FromKey(mostCommonCause.Key)
                )
            ));
        }
        return s_lines;
    }
}