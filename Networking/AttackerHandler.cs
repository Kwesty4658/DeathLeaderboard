using System.IO;

using DeathLeaderboard.Common.Systems;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DeathLeaderboard.Networking;

internal class AttackerHandler : PacketHandler
{
	internal const byte SyncLastAttacker = 1;

	internal AttackerHandler(byte handlerType) : base(handlerType) { }

	public override void HandlePacket(BinaryReader reader, int fromWho)
	{
		switch (reader.ReadByte()) {
			case SyncLastAttacker:
				ReceiveAttacker(reader, fromWho);
				break;
		}
	}

	internal void SendAttacker(string attackerName, int fromWho)
	{
		ModPacket packet = GetPacket(SyncLastAttacker, fromWho);

		packet.Write(attackerName);
		packet.Send();
	}

	private static void ReceiveAttacker(BinaryReader reader, int fromWho)
	{
		if (Main.netMode != NetmodeID.Server)
			return;

		string playerName = Main.player[fromWho].name;
		string attackerName = reader.ReadString();

		DeathsSavingSystem.AddDeath(playerName, attackerName);
	}
}