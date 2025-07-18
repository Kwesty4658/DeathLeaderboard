using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using DeathLeaderboard.Common.Configs;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using System;

namespace DeathLeaderboard.Common.Systems
{
    internal class StorageSystem : ModSystem
    {
        internal Dictionary<string, int> Deaths = [];

        string _path = Path.Combine(Main.SavePath, $"{Main.worldName}.json");

        public override void LoadWorldData(TagCompound tag)
        {
            if (!tag.ContainsKey("StorageType"))
                tag["StorageType"] = DeathLeaderboardConfig.StorageType.ToString();

            // Load player death data
            switch (DeathLeaderboardConfig.StorageType)
            {
                case StorageType.Json:
                    LoadJson();
                    return;

                case StorageType.Internal:
                    LoadInternal(tag);
                    return;
            }

            // If needed, migrate player death data
            if (Enum.Parse<StorageType>(tag.GetString("StorageType")) != DeathLeaderboardConfig.StorageType)
            {
                
            }
        }

        public override void SaveWorldData(TagCompound tag)
        {
            switch (DeathLeaderboardConfig.StorageType)
            {
                case StorageType.Json:
                    SaveJson();
                    return;

                case StorageType.Internal:
                    SaveInternal(tag);
                    return;
            }
        }

        public override void ClearWorld()
        {
            Deaths = [];
            _path = string.Empty;
        }

        private async void LoadJson()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_path));
            string json = await File.ReadAllTextAsync(_path);
            Deaths = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
        }

        private async void SaveJson()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_path));
            await File.WriteAllTextAsync(_path, JsonConvert.SerializeObject(Deaths, Formatting.Indented));
        }

        private void LoadInternal(TagCompound tag)
        {
            foreach (var player in tag.GetCompound("deaths"))
                Deaths[player.Key] = (int)player.Value;
        }

        private void SaveInternal(TagCompound tag)
        {
            TagCompound deaths = [];
            foreach (var player in Deaths)
                deaths[player.Key] = player.Value;

            tag["deaths"] = deaths;
        }
    }
}