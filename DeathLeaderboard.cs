using System.IO;
using log4net;
using Terraria.ModLoader;
using DeathLeaderboard.Networking;
using MonoMod.Logs;

namespace DeathLeaderboard
{
    public class DeathLeaderboard : Mod
    {
        internal static DeathLeaderboard Instance = ModContent.GetInstance<DeathLeaderboard>();
        internal static ILog Log = ModContent.GetInstance<DeathLeaderboard>().Logger;
        internal static AttackerHandler AttackerHandler;

        public override void Load()
        {
            AttackerHandler = new(AttackerHandler.SyncLastAttacker);
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI) => ModNetHandler.HandlePacket(reader, whoAmI);

    }
}