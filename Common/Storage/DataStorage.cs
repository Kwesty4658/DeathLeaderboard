using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace DeathLeaderboard.Common.Storage
{
    public class DataStorage : ModSystem
    {
        private string DataPath => Path.Combine(Main.SavePath, "DeathLeaderboard", Main.worldName, "data.json");
        private string DirectoryPath => Path.GetDirectoryName(DataPath);
        private Dictionary<string, int> _deathDataCache = new();

        public override void OnWorldLoad()
        {
            CheckDataAndDirectory();
            ReadDeathDisk();
        }
        public override void OnWorldUnload()
        {
            CheckDataAndDirectory();
            WriteDeathDisk();
        }

        public void WriteDeathDisk()
        {
            string json = JsonConvert.SerializeObject(_deathDataCache, Formatting.Indented);
            File.WriteAllText(DataPath, json);
        }
        public void ReadDeathDisk()
        {
            string json = File.ReadAllText(DataPath);
            _deathDataCache = JsonConvert.DeserializeObject<Dictionary<string, int>>(json) ?? new();
        }

        public void WriteDeathCache(Player player)
        {
            _deathDataCache[player.name] = _deathDataCache.GetValueOrDefault(player.name, 1) + 1;
        }
        public Dictionary<string, int> ReadDeathCache()
        {
            return _deathDataCache;
        }

        private void CheckDataAndDirectory()
        {
            Directory.CreateDirectory(DirectoryPath);

            if (!File.Exists(DataPath))
            {
                string emptyJson = JsonConvert.SerializeObject(_deathDataCache, Formatting.Indented);
                File.WriteAllText(DataPath, emptyJson);
            }
        }
    }
}
