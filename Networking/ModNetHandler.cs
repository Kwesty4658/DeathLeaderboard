using System.IO;

namespace DeathLeaderboard.Networking;

internal sealed class ModNetHandler
{
	internal const int AttackerType = 1;

	private static AttackerSyncHandler s_lastAttackerSync = new(AttackerType);

	internal static void HandlePacket(BinaryReader reader, int fromWho)
	{
		switch (reader.ReadByte())
		{
			case AttackerType:
				s_lastAttackerSync.HandlePacket(reader, fromWho);
				return;
		}
	}
}