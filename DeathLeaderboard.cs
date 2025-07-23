using System.IO;
using System.Xml.Linq;
using DeathLeaderboard.Networking;
using Terraria.ModLoader;

namespace DeathLeaderboard
{
    public class DeathLeaderboard : Mod
    {
        internal static DeathLeaderboard Instance = ModContent.GetInstance<DeathLeaderboard>();
        internal static SyncLastAttackerHandler LastAttackerHandler;

        public override void Load()
        {
            LastAttackerHandler = new(SyncLastAttackerHandler.SyncLastAttacker);
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI) => ModNetHandler.HandlePacket(reader, whoAmI);

    }
} 