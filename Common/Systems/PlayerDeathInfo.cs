using System;
using System.Collections.Generic;
using Terraria.ModLoader.IO;

namespace DeathLeaderboard.Common.Systems;

internal class LeaderboardPlayer : TagSerializable
{
    public string                  Name   { get; set; }
    public int                     Deaths { get; set; }
    public Dictionary<string, int> Causes { get; set; } = [];

    public TagCompound SerializeData()
    {
        return new TagCompound
        {
            { "name", Name },
            { "deaths", Deaths },
            { "causes", Causes }
        };
    }

    public static LeaderboardPlayer Load(TagCompound tag)
    {
        var player = new LeaderboardPlayer();
        player.Name = tag.GetString("name");
        player.Deaths = tag.GetInt("deaths");
        player.NestedCauses = tag.Get<NestedCauses>("nested");
    }

    public class NestedCauses : TagSerializable
    {
        public static readonly Func<TagCompound, NestedCauses> DESERIALIZER = Load;
    }
}

