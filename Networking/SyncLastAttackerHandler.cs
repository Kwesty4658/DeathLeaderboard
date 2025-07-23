using System.IO;
using DeathLeaderboard.Common.Players;
using Humanizer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DeathLeaderboard.Networking
{
    internal class SyncLastAttackerHandler : PacketHandler
    {
        internal const byte SyncLastAttacker = 1;

        internal SyncLastAttackerHandler(byte handlerType) : base(handlerType)
        {

        }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            switch (reader.ReadByte())
            {
                case SyncLastAttacker:
                    ReceiveAttacker(reader, fromWho);
                    break;
            }
        }

        internal void SendAttacker(string playerName, string attackerName, int fromWho)
        {
            ModPacket packet = GetPacket(SyncLastAttacker, fromWho);

            packet.Write(playerName);
            packet.Write(attackerName);
            packet.Send();
        }

        internal static void ReceiveAttacker(BinaryReader reader, int fromWho)
        {
            if (Main.netMode != NetmodeID.Server)
                return;

            string playerName = reader.ReadString();
            string attackerName = reader.ReadString();
            DeathTracker.UpdateServerAttackerInfo(playerName, attackerName);
            DeathTracker.DisplayLeaderboard();
        }
    }
}