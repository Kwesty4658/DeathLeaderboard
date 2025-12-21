using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Localization;

namespace DeathLeaderboard.Common.Systems;

internal sealed partial class LeaderboardSystem : ModSystem
{
    private static string                  s_jsonPath;
    private static List<LeaderboardPlayer> s_leaderboardPlayers;

    public override void OnWorldLoad()
    {
        if (Main.netMode == NetmodeID.MultiplayerClient) 
            return;
        
        s_jsonPath           = Path.Combine(Main.WorldPath, $"{Main.worldName}.deaths.json");
        s_leaderboardPlayers = [];

        if (Migrate()) 
            return;

        if (!File.Exists(s_jsonPath)) 
            File.WriteAllText(s_jsonPath, "[]");

        s_leaderboardPlayers = JsonConvert.DeserializeObject<List<LeaderboardPlayer>>(File.ReadAllText(s_jsonPath));
    }

    public override void OnWorldUnload()
    {
        if (!Main.dedServ || s_jsonPath == null || s_leaderboardPlayers == null)
            return;
        
        File.WriteAllText(s_jsonPath, JsonConvert.SerializeObject(s_leaderboardPlayers, Formatting.Indented));
        s_jsonPath           = null;
        s_leaderboardPlayers = null;
    }

    internal static void AddDeath(string playerName, PlayerDeathReason damageSource)
    {
        var npcInternalName   = GetLocalisationKey(damageSource);
        var leaderboardPlayer = s_leaderboardPlayers.FirstOrDefault(p => p.Name == playerName);

        if (leaderboardPlayer == null)
        {
            s_leaderboardPlayers.Add(new()
            {
                Name = playerName, 
                Deaths = 1, 
                Causes = new() { [npcInternalName] = 1 }
            });
            return;
        }

        leaderboardPlayer.Deaths++;
        leaderboardPlayer.Causes[npcInternalName] = leaderboardPlayer.Causes.TryGetValue(npcInternalName, out var count)
            ? count + 1
            : 1;

        DeathLeaderboard.Instance.Logger.Debug($"Death credited to {Language.GetText(npcInternalName)}");
    }


    private static bool Migrate()
    {
        var oldJsonPath = Path.Combine(Main.WorldPath, "..", "DeathLeaderboard", Main.worldName, "data.json");
        if (!File.Exists(oldJsonPath)) 
            return false;

        foreach (var kvp in JsonConvert.DeserializeObject<Dictionary<string, int>>(File.ReadAllText(oldJsonPath)))
            s_leaderboardPlayers.Add(new LeaderboardPlayer { Name = kvp.Key, Deaths = kvp.Value, Causes = [] });

        File.Delete(oldJsonPath);
        return true;
    }
}