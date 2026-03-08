using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader.IO;

namespace DeathLeaderboard.Common.Systems;

internal partial class LeaderboardSystem : ModSystem
{
    private static string                  s_jsonPath;
    private static List<LeaderboardPlayer> s_leaderboardPlayers;

    public override void LoadWorldData(TagCompound tag)
    {
        
    }

    public override void SaveWorldData(TagCompound tag)
    {
        
    }

    internal static void AddDeath(string playerName, PlayerDeathReason damageSource)
    {
        NetworkText causeText = GetLocalisationKey(damageSource);
        string causeKey  = causeText.ToString();                       
        LeaderboardPlayer leaderboardPlayer = s_leaderboardPlayers.FirstOrDefault(p => p.Name == playerName);

        if (leaderboardPlayer == null)
        {
            s_leaderboardPlayers.Add(new()
            {
                Name = playerName,
                Deaths = 1,
                Causes = new() { [causeKey] = 1 }
            });
            return;
        }

        leaderboardPlayer.Deaths++;
        leaderboardPlayer.Causes[causeKey] = leaderboardPlayer.Causes.TryGetValue(causeKey, out var count)
            ? count + 1
            : 1;

        DeathLeaderboard.Log.Debug($"Death credited to {causeText}");
    }
}