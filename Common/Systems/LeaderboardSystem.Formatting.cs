using System.Linq;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria;

namespace DeathLeaderboard.Common.Systems;

internal partial class LeaderboardSystem : ModSystem
{ 
    internal static List<NetworkText> Leaderboard()
    {
        List<NetworkText> lines = [ Header.ToNetworkText() ];

        foreach (var player in s_leaderboardPlayers.OrderByDescending(p => p.Deaths))
        {
            var mostCommonCause = player.Causes.OrderByDescending(c => c.Value).FirstOrDefault();

            NetworkText causeText = TryGetPlayerLiteral(mostCommonCause.Key, out var literal)
                ? NetworkText.FromLiteral(literal)
                : NetworkText.FromKey(mostCommonCause.Key ?? NoCauses.Key);

            if (mostCommonCause.Key == null)
            {
                lines.Add(NetworkText.FromKey(PlayerDeaths.Key, player.Name, player.Deaths, NetworkText.FromKey(NoCauses.Key)));
                continue;
            }

            lines.Add(NetworkText.FromKey(
                PlayerDeaths.Key,
                player.Name,
                player.Deaths,
                NetworkText.FromKey(DeathsTo.Key, mostCommonCause.Value, causeText)
            ));
        }
        return lines;
    }

    private static bool TryGetPlayerLiteral(string key, out string literal)
    {
        literal = key;
        return Main.player.Any(p => p?.active == true && p.name == key);
    }

}