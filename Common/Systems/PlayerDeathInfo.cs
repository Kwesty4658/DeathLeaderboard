using System.Collections.Generic;

namespace DeathLeaderboard.Common.Systems;

internal class Player
{
    public string Name { get; set; }
    public int Deaths { get; set; }
    public Dictionary<string, int> Causes { get; set; } = [];
}