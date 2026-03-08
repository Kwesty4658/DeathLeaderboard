using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace DeathLeaderboard.Common.GlobalNPCs
{
    public class NPCOwner : GlobalNPC
    {
        public int Owner { get; private set; }

        public override bool InstancePerEntity => true;
        public override void OnSpawn(NPC child, IEntitySource source)
        {
            if (source is not EntitySource_Parent { Entity: NPC parent }) return;

            if (parent.netID == -1)
            {
                Owner = parent.netID;
                return;
            }
            
            if (parent.netID != -1) Owner = Main.npc[parent.netID].netID;
        }
    }
}