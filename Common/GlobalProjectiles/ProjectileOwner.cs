using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace DeathLeaderboard.Common.GlobalProjectiles;

public sealed class ProjectileOwner : GlobalProjectile
{
	/// <summary>
	/// Projectile type, NPC netID
	/// </summary>
	public static Dictionary<int, int> ProjectileOwners = new();

	public override bool InstancePerEntity => true;

	public override void OnSpawn(Projectile projectile, IEntitySource source)
	{
		if (source is EntitySource_Parent { Entity: NPC { active: true } npc } && npc == Main.npc[npc.whoAmI])
		{
			ProjectileOwners[projectile.type] = npc.netID;
		}
	}
}