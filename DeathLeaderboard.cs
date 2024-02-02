using System.Threading.Tasks;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace DeathLeaderboard
{
    public class DeathLeaderboard : Mod
    {
        public static PlayerDeathData playerDeathData;
        public static Dictionary<string, int> playerDeaths;

        public override void Load()
        {
            playerDeathData = PlayerDeathData.Instance;
            playerDeaths = playerDeathData.PlayerDeaths;
        }
    }

    public class PlayerDeath : ModPlayer
    {
        public override async void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            // If player not in data, add base value of 1. If player in data, increment by 1.
            DeathLeaderboard.playerDeaths[Player.name] = DeathLeaderboard.playerDeaths.GetValueOrDefault(Player.name, 0) + 1;

            // save player deaths after updating
            DeathLeaderboard.playerDeathData.SavePlayerDeaths();

            // Short delay as the leaderboard is sent before the death message.
            await Task.Delay(500);
            Leaderboard.DisplayLeaderboard(DeathLeaderboard.playerDeaths);
        }
    }
}