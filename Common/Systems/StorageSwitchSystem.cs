using System;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using DeathLeaderboard.Common.Configs;

namespace DeathLeaderboard.Common.Systems
{
    public class StorageSwitchSystem : ModSystem
    {
        StorageType _type;

        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.ContainsKey("StorageType"))
                _type = Enum.Parse<StorageType>(tag.GetString("StorageType"));

            if (_type != DeathLeaderboardConfig.StorageType)
                Migrate();
        }

        public override void SaveWorldData(TagCompound tag)
        {

        }

        private void Migrate()
        {
            switch (_type)
            {
                case StorageType.Internal:
                    MigrateToJson();
                    return;

                case StorageType.Json:
                    MigrateToInternal();
                    return;
            }
        }

        
    }
}