using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;

namespace DeathLeaderboard
{
    public static class MessageHandler
    {
    private static bool IsOnline => Main.netMode == NetmodeID.Server;

        public static void SendMessage(string message, Color color)
        {
            NetworkText text = NetworkText.FromLiteral(message);

            if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(text, color);
            }
            else if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }
            else
            {
                Main.NewText(message, color);
            }
        }
    }
}