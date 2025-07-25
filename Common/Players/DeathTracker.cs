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
        private static readonly Color s_deathMsgColour = new(255, 25, 25);

        private bool _playerHasDied = false;
        private int _lastAttackerType = -1;

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            switch (Main.netMode)
            {
                case NetmodeID.Server:
                    return;

                case NetmodeID.MultiplayerClient:
                    DeathLeaderboard.AttackerHandler.SendAttacker(DeathCauseHelper.GetDeathCause(damageSource, _lastAttackerType), Player.whoAmI);
                    return;

                case NetmodeID.SinglePlayer:
                    _playerHasDied = true;
                    DeathsSavingSystem.AddDeath(Player.name, DeathCauseHelper.GetDeathCause(damageSource, _lastAttackerType));
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

        internal static void DisplayLeaderboard()
        {
            StringBuilder sb = new();
            sb.AppendLine("Leaderboard: ");

            foreach (var player in DeathsSavingSystem.Players)
            {
                var mostCommon = player.Causes
                    .OrderByDescending(c => c.Value)
                    .FirstOrDefault();

                sb.AppendLine(mostCommon.Key is not null
                    ? $"    {player.Name}: {player.Deaths} | {mostCommon.Value} deaths to {mostCommon.Key}"
                    : $"    {player.Name}: {player.Deaths} | No death causes recorded"
                );
            }

            switch (Main.netMode)
            {
                case NetmodeID.Server:
                    ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(sb.ToString()), s_deathMsgColour);
                    return;

                case NetmodeID.SinglePlayer:
                    Main.NewText(sb.ToString(), s_deathMsgColour);
                    return;

                case NetmodeID.MultiplayerClient:
                    return;

                default:
                    return;
            }
        }
    }
}