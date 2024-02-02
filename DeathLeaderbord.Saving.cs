using System.IO;
using Newtonsoft.Json;
using Terraria;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DeathLeaderboard
{
    public class PlayerDeathData
    {
        private string FilePath = Path.Combine(Main.SavePath, "DeathLeaderboard", "PlayerDeaths.json");
        
        // holds an instance of the class to be accessed internally
        private static PlayerDeathData instance;
    
        // class constructor - if LoadPlayerDeaths returns null (file doesn't exist) create new dict. 
        private PlayerDeathData()
        {
            PlayerDeaths = LoadPlayerDeaths() ?? new Dictionary<string, int>();
        }

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

        public Dictionary<string, int> PlayerDeaths { get; set; }

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
        private Dictionary<string, int> LoadPlayerDeaths()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                return JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
            }

            return null;
        }
    }
}