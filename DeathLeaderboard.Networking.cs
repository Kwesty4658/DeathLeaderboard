using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;

namespace DeathLeaderboard
{
    public static class MessageHandler
    {
        public static void SendMessage(string message, Color color)
        {
            NetworkText text = NetworkText.FromLiteral(message);

            if (!IsOnline())
            {
                Main.NewText(message, color);
            }
            else
            {
                ChatHelper.BroadcastChatMessage(text, color);
            }
        }

        private static bool IsOnline()
        {
            return Main.netMode == NetmodeID.MultiplayerClient || Main.netMode == NetmodeID.Server;
        }
    }
}