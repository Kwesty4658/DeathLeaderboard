using DeathLeaderboard.Common.Configs;

using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace DeathLeaderboard.Common.GlobalProjectiles;

public sealed class ProjectileParentGlobalProjectile : GlobalProjectile
{
	public override bool InstancePerEntity => true;

	public  NPC?           Parent;
	private IEntitySource? s_entitySource;

	public override void OnSpawn(Projectile projectile, IEntitySource source)
	{
		s_entitySource = source;

		const int recursionDepth = 5;
		int currentDepth = 0;

		// Recurse up the projectiles owner chain until it is an npc
		while (source is EntitySource_Parent parentSource && currentDepth++ < recursionDepth)
		{
			switch (parentSource.Entity)
			{
				case Projectile parentProjectile:
				{
					if (parentProjectile.active)
					{
						source = parentProjectile.GetGlobalProjectile<ProjectileParentGlobalProjectile>()?.s_entitySource;

						if (source != null)
							break;
					}
					Parent = null;
					return;
				}

				case NPC parentNPC:
				{
					if (parentNPC.active)
					{
						Parent = parentNPC;
						return;
					}
					Parent = null;
					return;
				}

				default:
				{
					Parent = null;
					return;
				}
			}
		}
	}
}