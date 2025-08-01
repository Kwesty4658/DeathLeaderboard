using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

using DeathLeaderboard.Common.Systems;

namespace DeathLeaderboard.Common.Players;

internal sealed class LeaderboardPlayer : ModPlayer
{
    public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
    {
	    if (Main.dedServ)
		    return;

	    switch (Main.netMode)
	    {
		    case NetmodeID.MultiplayerClient:
			    DeathLeaderboard.AttackerSyncHandler.Send(
				    Player.whoAmI,
				    LeaderboardLocalisation.GetLocalisationKey(damageSource));
			    return;

		    case NetmodeID.SinglePlayer:
			    LeaderboardSystem.AddDeath(
				    Player.name,
				    LeaderboardLocalisation.GetLocalisationKey(damageSource));
			    return;
	    }
    }

    public override void OnRespawn()
    {
        switch (Main.netMode)
        {
            case NetmodeID.Server:
                foreach (NetworkText line in Leaderboard.Get())
                    ChatHelper.BroadcastChatMessage(line, Color.Red);
                return;

            case NetmodeID.MultiplayerClient:
                return;

            case NetmodeID.SinglePlayer:
                foreach (NetworkText line in Leaderboard.Get())
                    Main.NewText(line, Color.Red);
                return;
        }
    }
}