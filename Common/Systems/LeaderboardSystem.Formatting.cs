using System.Linq;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.Localization;

namespace DeathLeaderboard.Common.Systems;

internal sealed partial class LeaderboardSystem : ModSystem
{ 
    internal static List<NetworkText> Leaderboard()
    {    
        List<NetworkText> lines = [ Header.ToNetworkText() ];

        foreach (var player in s_leaderboardPlayers.OrderByDescending(p => p.Deaths))
        {
            var mostCommonCause = player.Causes.OrderByDescending(c => c.Value).FirstOrDefault();

            if (mostCommonCause.Key == null)
            {
                lines.Add(NetworkText.FromKey(
                    PlayerDeaths.Key, // {player.Name}: {player.Deaths} | {s_noCauses.Key}
                    player.Name,
                    player.Deaths,
                    NetworkText.FromKey(NoCauses.Key)
                ));
                continue;
            }

            lines.Add(NetworkText.FromKey(
                PlayerDeaths.Key, // {player.Name} deaths to {player.Deaths} | {s_deathsTo.Key}
                player.Name,
                player.Deaths,
                NetworkText.FromKey(DeathsTo.Key, mostCommonCause.Value, NetworkText.FromKey(mostCommonCause.Key)
                )
            ));
        }
        return lines;
    }
}