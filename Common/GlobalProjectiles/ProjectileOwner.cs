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
    /// Maps projectile types to the entity that spawned them in.
    /// Dictionary<projectile.type, parent.netID>
    /// </summary>
    public static Dictionary<int, int> Owners { get; } = [];

    public override bool InstancePerEntity => true;

    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        if (source is EntitySource_Parent { Entity: NPC { } parent })
        {
            DeathLeaderboard.Log.Debug($"{Lang.GetNPCName(parent.netID)} spawned projectile: {Lang.GetProjectileName(projectile.type)}");

            if (parent.realLife < -1)
            {
                Owners[projectile.type] = parent.realLife;
                return;
            }
            
            Owners[projectile.type] = parent.netID;
        }
    }
}