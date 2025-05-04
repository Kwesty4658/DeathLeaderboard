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
        private DataStorage Data => ModContent.GetInstance<DataStorage>();

        private bool _died = false;

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource) => _died = true;

        public override void PostUpdate() {
            if (_died) {
                _died = false;
                Data.Write(Player.name);
                SendMessage();
            }
        }

        private void SendMessage() {
            var sortedData = from entry in Data.GetData() orderby entry.Value descending select entry;

            var sb = new StringBuilder();
            sb.AppendLine("Death Leaderboard:");
            foreach (var kvp in sortedData) {
                sb.AppendLine($"  {kvp.Key}: {kvp.Value}");
            }

            switch (Main.netMode) {
                case NetmodeID.SinglePlayer:
                    Main.NewText(sb.ToString(), Color.Red);
                    return;
                case NetmodeID.MultiplayerClient:
                    return;
                case NetmodeID.Server:
                    NetworkText text = NetworkText.FromLiteral(sb.ToString());
                    ChatHelper.BroadcastChatMessage(text, Color.Red);
                    return;
                default:
                    return;
            }
        }
    }
}