using System.IO;

using DeathLeaderboard.Common.Systems;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DeathLeaderboard.Networking;

internal class AttackerSyncHandler : PacketHandler
{
	internal const byte SyncLastAttacker = 1;

	internal AttackerSyncHandler(byte handlerType) : base(handlerType) { }

	public override void HandlePacket(BinaryReader reader, int fromWho)
	{
		switch (reader.ReadByte())
		{
			case SyncLastAttacker:
				Receive(reader, fromWho);
				break;
		}
	}

	internal static void Send(int whoAmI, string key)
	{
		Main.NewText($"{Main.player[whoAmI].name} sync last attacker: {key}");

		ModPacket packet = DeathLeaderboard.Instance.GetPacket();
		packet.Write(key);
		packet.Send();
	}

	private static void Receive(BinaryReader reader, int fromWho)
	{
		if (Main.netMode != NetmodeID.Server)
			return;

		string key = reader.ReadString();

		ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"{Main.player[fromWho].name} sync last attacker: {key}"),  Color.Blue);

		LeaderboardSystem.AddDeath(Main.player[fromWho].name, key);
	}
}