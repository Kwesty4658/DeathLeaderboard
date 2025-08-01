using System.Linq;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

using DeathLeaderboard.Common.Systems;

using Player = DeathLeaderboard.Common.Systems.Player;

namespace DeathLeaderboard.Common;

internal sealed class Leaderboard : ModSystem
{
    private static List<NetworkText> s_lines;
    private static LocalizedText s_header, s_playerDeaths, s_deathsTo, s_noCauses;

    public override void OnWorldLoad()
    {
        s_header        = Mod.GetLocalization("Leaderboard.Header");
        s_playerDeaths  = Mod.GetLocalization("Leaderboard.PlayerDeaths");
        s_deathsTo      = Mod.GetLocalization("Leaderboard.DeathsTo");
        s_noCauses      = Mod.GetLocalization("Leaderboard.NoCauses");
    }

    /// <summary>
    /// The actual localisation (as NetworkText) ends here. The non-localised stored values are
    /// transformed into NetworkText.
    /// </summary>
    /// <returns>The leaderboard lines formatted as NetworkText</returns>
    internal static List<NetworkText> Get()
    {
        s_lines = [s_header.ToNetworkText()];

        foreach (Player player in LeaderboardSystem.Players)
        {
            KeyValuePair<string, int> mostCommonCause = player.Causes
                .OrderByDescending(c => c.Value)
                .FirstOrDefault();

            if (mostCommonCause.Key == null)
            {
	            s_lines.Add(NetworkText.FromKey(
		            s_playerDeaths.Key,	// {player.Name}: {player.Deaths} | {s_noCauses.Key}
		            player.Name,
		            player.Deaths,
		            NetworkText.FromKey(s_noCauses.Key)
	            ));
	            continue;
            }

            s_lines.Add(NetworkText.FromKey(
	            s_playerDeaths.Key,	// {player.Name} deaths to {player.Deaths} | {s_deathsTo.Key}
	            player.Name,
	            player.Deaths,
	            NetworkText.FromKey(
		            s_deathsTo.Key,	// {mostCommonCause.Value} deaths to {cause}
		            mostCommonCause.Value,
		            NetworkText.FromKey(mostCommonCause.Key)
	            )
            ));
        }
        return s_lines;
    }
}