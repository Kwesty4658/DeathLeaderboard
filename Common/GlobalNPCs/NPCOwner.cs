using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace DeathLeaderboard.Common.GlobalNPCs
{
    public class NPCOwner : GlobalNPC
    {
        public static Dictionary<int, int> Owners { get; } = [];

        public override bool InstancePerEntity => true;

        // NOTE: storing by the localisation key is probably a lot more reliable
        public override void OnSpawn(NPC child, IEntitySource source)
        {
            if (source is EntitySource_Parent { Entity: NPC { } parent })
            {
                DeathLeaderboard.Log.Debug($"{Lang.GetNPCName(parent.netID)} spawned child: {Lang.GetNPCName(child.netID)}");

                if (parent.realLife < -1)
                {
                    Owners[child.netID] = parent.realLife;
                    return;
                }

                Owners[child.netID] = parent.netID;
                return;
            }
        }
    }
}