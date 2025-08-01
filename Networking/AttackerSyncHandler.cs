using System.IO;

using DeathLeaderboard.Common.Systems;

using Terraria;
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

	internal void Send(int whoAmI, string key)
	{
		ModPacket packet = GetPacket(SyncLastAttacker, whoAmI);
		packet.Write(key);
		packet.Send();
	}

	private static void Receive(BinaryReader reader, int fromWho)
	{
		if (Main.netMode != NetmodeID.Server)
			return;

		LeaderboardSystem.AddDeath(Main.player[fromWho].name, reader.ReadString());
	}
}