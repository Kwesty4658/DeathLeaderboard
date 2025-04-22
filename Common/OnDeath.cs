using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using DeathLeaderboard.Common.Storage;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Chat;
using System.Text;
using System.Threading.Tasks;

namespace DeathLeaderboard.Common
{
    public class OnDeath : ModPlayer
    {
        private Color _color = Color.Red;

        public override async void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            var data = ModContent.GetInstance<DataStorage>();

            data.WriteDeathCache(Player);
            data.WriteDeathDisk();

            await Task.Delay(500);
            SendMessage(data.ReadDeathCache());
        }

        private void SendMessage(Dictionary<string, int> data)
        {
            var sortedData = from entry in data orderby entry.Value descending select entry;
            StringBuilder sb = new ();
            sb.AppendLine("Death Leaderboard:");

            foreach (KeyValuePair<string, int> kvp in sortedData)
            {
                sb.AppendLine($"  {kvp.Key}: {kvp.Value}");
            }

            string message = sb.ToString();

            NetworkText text = NetworkText.FromLiteral(message);

            if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(text, _color);
            }
            else if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }
            else
            {
                Main.NewText(message, _color);
            }
        }
    }
}