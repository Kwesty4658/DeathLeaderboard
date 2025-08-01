using System.IO;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DeathLeaderboard.Networking;

internal abstract class PacketHandler(byte handlerType)
{
	internal byte HandlerType { get; set; } = handlerType;

	public abstract void HandlePacket(BinaryReader reader, int fromWho);

	protected ModPacket GetPacket(byte packetType, int fromWho)
	{
		ModPacket packet = DeathLeaderboard.Instance.GetPacket();

		packet.Write(HandlerType);
		packet.Write(packetType);
		if (Main.netMode == NetmodeID.Server)
			packet.Write((byte)fromWho);

		return packet;
	}
}