using DeathLeaderboard.Common.GlobalNPCs;
using DeathLeaderboard.Common.GlobalProjectiles;
using DeathLeaderboard.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DeathLeaderboard.Common.Players;

internal sealed class LeaderboardPlayer : ModPlayer
{
    public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
    {
        switch (Main.netMode)
        {
            case NetmodeID.Server:
            case NetmodeID.SinglePlayer:
                LeaderboardSystem.AddDeath(Player.name, damageSource);
                return;

            case NetmodeID.MultiplayerClient:
                return;
        }
    }

    public override void OnRespawn()
    {
        switch (Main.netMode)
        {
            case NetmodeID.Server:
            case NetmodeID.SinglePlayer:
                foreach (var line in LeaderboardSystem.Leaderboard())
                    ChatHelper.BroadcastChatMessage(line, Color.Red);
                break;

            case NetmodeID.MultiplayerClient:
                return;
        }

        NPCOwner.Owners.Clear();
        ProjectileOwner.Owners.Clear();
    }
}