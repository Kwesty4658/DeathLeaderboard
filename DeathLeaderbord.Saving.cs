using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Terraria;
using System.Collections.Concurrent;

namespace DeathLeaderboard
{
    public class PlayerDeathData
    {
        // private static variable that holds a single instance of the class
        private static PlayerDeathData instance;

        // creates an instance of the class if one has not already been created
        public static PlayerDeathData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayerDeathData();
                }
                return instance;
            }
        }

        // attempts to load player deaths from the json file. If file not found, a new dict is instantiated
        private PlayerDeathData()
        {
            PlayerDeaths = LoadPlayerDeaths() ?? new ConcurrentDictionary<string, int>();
        }

        // combines the mod name with the main save path of tmodloader
        private static string FilePath => Path.Combine(Main.SavePath, "DeathLeaderboard", "PlayerDeaths.json");
        public ConcurrentDictionary<string, int> PlayerDeaths { get; private set; }

        public void SavePlayerDeaths()
        {
            string directory = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            string json = JsonConvert.SerializeObject(PlayerDeaths, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        // loads player deaths into a dictionary 
        private ConcurrentDictionary<string, int> LoadPlayerDeaths()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                return JsonConvert.DeserializeObject<ConcurrentDictionary<string, int>>(json);
            }

            return null;
        }
    }
}