using System.IO;

namespace DeathLeaderboard.Networking
{
    internal class ModNetHandler
    {
        internal const int AttackerType = 1;

        private static SyncLastAttackerHandler _lastAttacker = new(AttackerType);

        internal static void HandlePacket(BinaryReader reader, int fromWho)
        {
            switch (reader.ReadByte())
            {
                case AttackerType:
                    _lastAttacker.HandlePacket(reader, fromWho);
                    return;
            }
        }
    }
}