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
using Mono.Cecil.Cil;

namespace DeathLeaderboard.Common
{
    public class OnDeath : ModPlayer
    {
        
        private Color _color = Color.Red;

        public override async void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            var dataStorage = ModContent.GetInstance<DataStorage>();

            if (dataStorage.DeathData == null)
            {
                dataStorage.DeathData = new Dictionary<string, int>();
            }

            dataStorage.DeathData[Player.name] = dataStorage.DeathData.GetValueOrDefault(Player.name, 1) + 1;
            dataStorage.SaveOnDeath();

            await Task.Delay(500);
            SendMessage(dataStorage.DeathData);
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