using System.Linq;
using System.Text;

using DeathLeaderboard.Common.Systems;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DeathLeaderboard.Common.Players;

internal sealed class DeathTracker : ModPlayer
{
	private int _lastAttackerType = -1;

	private static readonly Color s_deathMsgColour = new(255, 25, 25);

	private static LocalizedText DeathsTo => Language.GetOrRegister("Mods.DeathLeaderboard.Leaderboard.DeathsTo");

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
				DeathsSavingSystem.AddDeath(Player.name, DeathCauseHelper.GetDeathCause(damageSource, _lastAttackerType));
				return;
		}
	}

	public override void OnRespawn()
	{
		DisplayLeaderboard();
	}

	public override void OnHitByNPC(NPC npc, Terraria.Player.HurtInfo hurtInfo)
	{
		_lastAttackerType = npc.type;
	}


	// Need to do this since ModSystem.OnWorldLoad() isn't called on singleplayer clients.
	public override void OnEnterWorld()
	{
		if (Main.netMode == NetmodeID.SinglePlayer)
			DeathsSavingSystem.ReadData();
	}

	internal static void DisplayLeaderboard()
	{
		switch (Main.netMode)
		{
			case NetmodeID.Server:
				ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(FormatLeaderboard()), s_deathMsgColour);
				return;

			case NetmodeID.SinglePlayer:
				Main.NewText(FormatLeaderboard(), s_deathMsgColour);
				return;

			case NetmodeID.MultiplayerClient:
				return;

			default:
				return;
		}
	}

	private static string FormatLeaderboard()
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
		return sb.ToString();
	}
}