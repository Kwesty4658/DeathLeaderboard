using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace DeathLeaderboard.Common.Storage
{
    public class DataStorage : ModSystem
    {
        private string _dataPath => Path.Combine(Main.SavePath, "DeathLeaderboard", Main.worldName, "data.json");
        private string _directoryPath => Path.GetDirectoryName(_dataPath);
        private Dictionary<string, int> _data = [];

        public override void OnWorldLoad() {
            Directory.CreateDirectory(_directoryPath);
            Read();
        } 

        public void Read() {
            string j = File.ReadAllText(_dataPath);
            _data = JsonConvert.DeserializeObject<Dictionary<string, int>>(j) ?? [];
        }
        public void Write(string player) {
            if (!File.Exists(_dataPath)) {
                Directory.CreateDirectory(_directoryPath);
                File.WriteAllText(_dataPath, "{}");
            }
            _data[player] = _data.GetValueOrDefault(player, 1) + 1;
            string j = JsonConvert.SerializeObject(_data, Formatting.Indented);
            File.WriteAllText(_dataPath, j);
        }

        public Dictionary<string, int> GetData() => _data;
    }
}
