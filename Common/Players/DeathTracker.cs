using System.Text;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Chat;
using Terraria.Localization;
using DeathLeaderboard.Common.Systems;
using System.Linq;


namespace DeathLeaderboard.Common.Players
{
    public class DeathTracker : ModPlayer
    {
        private static readonly Color DeathChatColour = new(255, 25, 25);
        private bool _playerHasDied = false;

        private int _lastAttackerType = -1;

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            switch (Main.netMode)
            {
                case NetmodeID.Server:
                    return;

                case NetmodeID.MultiplayerClient:
                    Main.NewText("Multiplayer Client");
                    DeathLeaderboard.LastAttackerHandler.SendAttacker(Player.name, DeathCauseHelper.GetDeathCause(damageSource, _lastAttackerType), Player.whoAmI);
                    return;

                case NetmodeID.SinglePlayer:
                    Main.NewText("Singleplayer");
                    _playerHasDied = true;
                    DeathSystem.AddDeath(Player.name, DeathCauseHelper.GetDeathCause(damageSource, _lastAttackerType));
                    return;
            }
        }

        public override void OnHitByNPC(NPC npc, Terraria.Player.HurtInfo hurtInfo) => _lastAttackerType = npc.type;

        public override void PostUpdate()
        {
            if (_playerHasDied)
                DisplayLeaderboard();

            _playerHasDied = false;
        }

        internal static void UpdateServerAttackerInfo(string playerName, string attackerName) => DeathSystem.AddDeath(playerName, attackerName);

        internal static void DisplayLeaderboard()
        {
            StringBuilder sb = new();
            sb.AppendLine("Leaderboard: ");

            foreach (var player in DeathSystem.Players)
            {
                var mostCommon = player.Causes
                    .OrderByDescending(c => c.Value)
                    .FirstOrDefault();

                string msg = mostCommon.Key is not null
                    ? $"    {player.Name}: {player.DeathSystem} | {mostCommon.Value} DeathSystem to {mostCommon.Key}"
                    : $"    {player.Name}: {player.DeathSystem} | No death causes recorded";

                sb.AppendLine(msg);
            }

            switch (Main.netMode)
            {
                case NetmodeID.Server:
                    ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(sb.ToString()), DeathChatColour);
                    return;

                case NetmodeID.SinglePlayer:
                    Main.NewText(sb.ToString(), DeathChatColour);
                    return;

                case NetmodeID.MultiplayerClient:
                    return;

                default:
                    return;
            }
        }
    }
}