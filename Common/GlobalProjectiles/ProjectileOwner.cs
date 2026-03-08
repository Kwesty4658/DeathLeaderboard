using System;
using System.Collections.Generic;
using System.Numerics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace DeathLeaderboard.Common.GlobalProjectiles;

public class ProjectileOwner : GlobalProjectile
{
    /// <summary>
    /// netID of the owning entity
    /// </summary>
    public int Owner { get; private set; }

    public override bool InstancePerEntity => true;

    public override void OnSpawn(Projectile projectile, IEntitySource source)
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