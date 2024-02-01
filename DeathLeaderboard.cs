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

            playerDeathData.PlayerDeaths.AddOrUpdate(Player.name, 1, (key, oldValue) => oldValue + 1);            

            // save player deaths after updating
            playerDeathData.SavePlayerDeaths();

            await Task.Delay(500);
            Leaderboard.DisplayLeaderboard(playerDeathData.PlayerDeaths);
        }
    }
}