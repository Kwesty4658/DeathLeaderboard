using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;
using Newtonsoft.Json;
using Terraria;
using Terraria.ModLoader;

namespace DeathLeaderboard.Common.Systems;

[Autoload(Side = ModSide.Server)]
internal sealed class DeathsSavingSystem : ModSystem
{
    internal static List<Player> Players { get; private set; } = [];

    private static string s_jsonPath;

    private static readonly ILog s_log = DeathLeaderboard.Log;

    public override void OnWorldLoad()
    {
        ReadData();
    }

    public override void OnWorldUnload()
    {
        File.WriteAllText(s_jsonPath, JsonConvert.SerializeObject(Players, Formatting.Indented));
    }

    public override void ClearWorld()
    {
        Players = null;
    }

    internal static void AddDeath(string playerName, string npcName)
    {
        var player = Players.FirstOrDefault(p => p.Name == playerName);

        if (player == null)
        {
            Players.Add(new Player
            {
                Name = playerName,
                Deaths = 1,
                Causes = new Dictionary<string, int> { [npcName] = 1 }
            });
            return;
        }

        player.Deaths++;
        player.Causes[npcName] = player.Causes.TryGetValue(npcName, out int count) ? count + 1 : 1;
    }

    internal static void ReadData()
    {
        s_jsonPath = Path.Combine(Path.GetDirectoryName(Main.ActiveWorldFileData.Path), $"{Main.ActiveWorldFileData.Path}.Deaths.json");

        if (Migrate())
            return;

        if (!File.Exists(s_jsonPath))
            File.WriteAllText(s_jsonPath, "[]");

        Players = JsonConvert.DeserializeObject<List<Player>>(File.ReadAllText(s_jsonPath));
    }

    private static bool Migrate()
    {
        s_log.Info("Checking for migration...");

        string oldJsonPath = Path.Combine(Main.SavePath, "Deathleaderboard", Main.worldName, "data.json");

        if (File.Exists(oldJsonPath))
        {
            s_log.Info("Old data exists! Migrating now.");

            Dictionary<string, int> oldData = JsonConvert.DeserializeObject<Dictionary<string, int>>(File.ReadAllText(oldJsonPath));
            foreach (var kvp in oldData)
            {
                Players.Add(new Player
                {
                    Name = kvp.Key,
                    Deaths = kvp.Value,
                    Causes = []
                });
            }
            s_log.Info("Migration complete. Deleting old data.");

            File.Delete(oldJsonPath);
            return true; // true if migration happened
        }

        s_log.Info("Migration not requried, continuing with loading.");
        return false; // false if migration didn't happen
    }
}
