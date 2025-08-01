using System.IO;
using System.Text.RegularExpressions;

using DeathLeaderboard.Common.GlobalProjectiles;
using DeathLeaderboard.Networking;

using Humanizer;

using log4net;

using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace DeathLeaderboard;

internal sealed class DeathLeaderboard : Mod
{
	internal static readonly DeathLeaderboard Instance = ModContent.GetInstance<DeathLeaderboard>();
	internal static readonly ILog Log = ModContent.GetInstance<DeathLeaderboard>().Logger;
	internal static AttackerSyncHandler AttackerSyncHandler;

	public override void Load()
	{
		AttackerSyncHandler = new(AttackerSyncHandler.SyncLastAttacker);
	}

	public override void HandlePacket(BinaryReader reader, int whoAmI) => ModNetHandler.HandlePacket(reader, whoAmI);

}