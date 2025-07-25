using System.Linq;
using System.Text;

using Terraria.Localization;
using Terraria.ModLoader;

namespace DeathLeaderboard.Common.Systems;

internal sealed class LeaderboardSystem : ModSystem
{
    private static string s_deathLine;
    private static string s_deathsTo;
    private static string s_noCauses;

    public override void PostSetupContent()
    {
        s_deathLine = Language.GetOrRegister("Mods.DeathLeaderboard.Leaderboard.DeathLine").Value;
        s_deathsTo = Language.GetOrRegister("Mods.DeathLeaderboard.Leaderboard.DeathsTo").Value;
        s_noCauses = Language.GetOrRegister("Mods.DeathLeaderboard.Leaderboard.NoCauses").Value;
    }

    internal static string Leaderboard()
    {
        StringBuilder sb = new();

        sb.AppendLine("=====LEADERBOARD=====");
        foreach (Player player in DeathsSavingSystem.Players)
        {
            var mostCommon = player.Causes
                .OrderByDescending(c => c.Value)
                .FirstOrDefault();

            sb.AppendLine(mostCommon.Key is not null
                ? $"    {player.Name}: {player.Deaths} | {mostCommon.Value} {s_deathsTo} {mostCommon.Key}"
                : $"    {player.Name}: {player.Deaths} | No death causes recorded"
            );
        }

        return sb.ToString();
    }
}