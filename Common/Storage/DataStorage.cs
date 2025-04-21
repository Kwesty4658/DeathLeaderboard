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
        public Dictionary<string, int> DeathData { get; set; } = new();

        private string _dataLocation;

        public override void OnWorldLoad()
        {
            _dataLocation = Path.Combine(Main.SavePath, "DeathLeaderboard", Main.worldName, "data.json");
            string directory = Path.GetDirectoryName(_dataLocation);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (!File.Exists(_dataLocation))
            {
                DeathData = new Dictionary<string, int>();
                string emptyJson = JsonConvert.SerializeObject(DeathData, Formatting.Indented);
                File.WriteAllText(_dataLocation, emptyJson);
            }
            else
            {
                string json = File.ReadAllText(_dataLocation);
                DeathData = JsonConvert.DeserializeObject<Dictionary<string, int>>(json) ?? new Dictionary<string, int>();
            }

            Main.NewText("Death leaderboard data loaded.", Color.LightGreen);
        }

        public override void OnWorldUnload()
        {
            SaveData();
        }

        public void SaveOnDeath()
        {
            SaveData();
        }

        private void SaveData()
        {
            if (string.IsNullOrEmpty(_dataLocation))
                return;

            string json = JsonConvert.SerializeObject(DeathData, Formatting.Indented);
            File.WriteAllText(_dataLocation, json);
        }
    }
}
