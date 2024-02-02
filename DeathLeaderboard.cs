using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace DeathLeaderboard
{
    public class DeathCounter : ModPlayer
    {
        public override async void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            // accesses the singleton instance of playerDeathData
            var playerDeathData = PlayerDeathData.Instance;

            playerDeathData.PlayerDeaths[Player.name] = playerDeathData.PlayerDeaths.GetValueOrDefault(Player.name, 0) + 1;

            // save player deaths after updating
            playerDeathData.SavePlayerDeaths();

            await Task.Delay(500);
            Leaderboard.DisplayLeaderboard(playerDeathData.PlayerDeaths);
        }
    }
}