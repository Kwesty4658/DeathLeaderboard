using log4net;
using Terraria.ModLoader;

namespace DeathLeaderboard;

internal sealed class DeathLeaderboard : Mod
{
    internal static readonly DeathLeaderboard Instance = ModContent.GetInstance<DeathLeaderboard>();
    internal static readonly ILog Log = Instance.Logger;
}