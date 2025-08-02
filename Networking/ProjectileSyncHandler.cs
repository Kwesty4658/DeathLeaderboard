using System.IO;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DeathLeaderboard.Networking;

internal class ProjectileSyncHandler : PacketHandler
{
	internal const byte SyncProjectileOwner = 2;

	internal ProjectileSyncHandler(byte handlerType) : base(handlerType) { }

	public override void HandlePacket(BinaryReader reader, int fromWho)
	{
		switch (reader.ReadByte())
		{
			case SyncProjectileOwner:
			{
				Receive(reader, fromWho);
				return;
			}
		}
	}

	// Client asks: who owns this projectile type?
	internal static void GetProjectileType_Client(int whoAmI, int projectileType)
	{
		if (Main.netMode != NetmodeID.MultiplayerClient)
			return;

		ModPacket packet = DeathLeaderboard.Instance.GetPacket();
		packet.Write(projectileType);
		packet.Send();
	}

	private static void RespondProjectileOwner_Server(BinaryReader reader, int fromWho)
	{

	}

	private static void Receive(BinaryReader reader, int fromWho)
	{

	}
}