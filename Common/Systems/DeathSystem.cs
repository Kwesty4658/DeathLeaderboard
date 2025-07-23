using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace DeathLeaderboard.Common.Systems
{
    [Autoload(Side = ModSide.Server)]
    internal class DeathSystem : ModSystem
    {
        internal static List<Player> Players { get; private set; }

        private string _jsonData;
        private string _oldJsonData;

        public override void OnWorldLoad()
        {
            Players = [];
            _jsonData = Path.Combine(Main.WorldPath, $"{Main.worldName}.DeathSystem.json");

            if (NeedsMigration())
            {
                Migrate();
                return;
            }

            if (!File.Exists(_jsonData))
                File.WriteAllText(_jsonData, "[]");

            Players = JsonConvert.DeserializeObject<List<Player>>(File.ReadAllText(_jsonData));
        }

        public override void SaveWorldData(TagCompound tag) => File.WriteAllText(_jsonData, JsonConvert.SerializeObject(Players, Formatting.Indented));

        internal static void AddDeath(string playerName, string npcName)
        {
            var player = Players.FirstOrDefault(p => p.Name == playerName);

            if (player is not null)
            {
                player.DeathSystem++;

                if (player.Causes.TryGetValue(npcName, out int value))
                    player.Causes[npcName] = ++value;

                else
                    player.Causes[npcName] = 1;

                return;
            }
            else
            {
                Players.Add(new Player
                {
                    Name = playerName,
                    DeathSystem = 1,
                    Causes = new Dictionary<string, int>
                    {
                        [npcName] = 1
                    }
                });
            }
        }

        private static bool NeedsMigration()
        {
            _oldJsonData = Path.Combine(Main.SavePath, "DeathLeaderboard", Main.worldName, "data.json");

            if (File.Exists(_oldJsonData))
                return true;

            return false;
        }

        private static void Migrate()
        {
            Players ??= [];

            string json = File.ReadAllText(_oldJsonData);
            Dictionary<string, int> oldData = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);

            foreach (var kvp in oldData)
            {
                Players.Add(new Player
                {
                    Name = kvp.Key,
                    DeathSystem = kvp.Value,
                    Causes = []
                });
            }

            File.Delete(_oldJsonData);
        }
    }
}
