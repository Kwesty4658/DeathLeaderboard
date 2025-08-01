using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace DeathLeaderboard.Common.GlobalProjectiles;

public sealed class ProjectileParentGlobalProjectile : GlobalProjectile
{
	public NPC Parent;

	private IEntitySource s_source = null;

	public override bool InstancePerEntity => true;

	public override void OnSpawn(Projectile projectile, IEntitySource source)
	{
		s_source = source;
		FindParent(source);
	}

	// Wish I had this.
	private void FindParent(IEntitySource? source)
	{
		const int maxDepth = 10;
		int depth = 0;

		while (source is EntitySource_Parent parentSource && depth++ < maxDepth)
		{
			Entity entity = parentSource.Entity;

			switch (entity)
			{
				case Projectile projectile:
					if (!projectile.active)
					{
						Parent = null;
						return;
					}

					var projGlobal = projectile.GetGlobalProjectile<ProjectileParentGlobalProjectile>();
					source = projGlobal?.s_source;

					if (source == null)
					{
						Parent = null;
						return;
					}
					break;

				case NPC npc:
					if (!npc.active)
					{
						Parent = null;
						return;
					}

					Parent = npc;
					return;

				default:
					Parent = null;
					return;
			}
		}
	}
}