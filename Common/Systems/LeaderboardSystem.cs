using System.Collections.Generic;
using System.IO;
using System.Linq;

using log4net;

using Newtonsoft.Json;

using Terraria;
using Terraria.ModLoader;

namespace DeathLeaderboard.Common.Systems;

internal sealed partial class LeaderboardSystem : ModSystem
{
	internal static List<Player> Players { get; private set; }

	private static string s_jsonPath;

	private static readonly ILog s_log = DeathLeaderboard.Log;

	public override void OnWorldLoad()
	{
		Players = [];

		s_jsonPath = Path.Combine(Main.WorldPath, $"{Main.worldName}.deaths.json");

		if (Migrate())
			return;

		if (!File.Exists(s_jsonPath))
			File.WriteAllText(s_jsonPath, "[]");

		Players = JsonConvert.DeserializeObject<List<Player>>(File.ReadAllText(s_jsonPath));
	}

	public override void OnWorldUnload()
	{
		File.WriteAllText(s_jsonPath, JsonConvert.SerializeObject(Players, Formatting.Indented));

		Players = null;
		s_jsonPath = null;
	}

	internal static void AddDeath(string playerName, string npcInternalName)
	{
		Player player = Players.FirstOrDefault(p => p.Name == playerName);

		if (player == null)
		{
			Players.Add(new Player
			{
				Name = playerName,
				Deaths = 1,
				Causes = new Dictionary<string, int> { [npcInternalName] = 1 }
			});
			return;
		}

		player.Deaths++;
		player.Causes[npcInternalName] = player.Causes.TryGetValue(npcInternalName, out int count) ? count + 1 : 1;
	}

	private static bool Migrate()
	{
		s_log.Info("Checking for migration...");

		string oldJsonPath = Path.Combine(Main.WorldPath, "..", "DeathLeaderboard", Main.worldName, "data.json");
		s_log.Info($"old world path is {oldJsonPath}");

		if (File.Exists(oldJsonPath))
		{
			s_log.Info("Old data exists! Migrating now.");

			var oldData = JsonConvert.DeserializeObject<Dictionary<string, int>>(File.ReadAllText(oldJsonPath));
			foreach (var kvp in oldData)
			{
				Players.Add(new Player
				{
					Name = kvp.Key,
					Deaths = kvp.Value,
					Causes = []
				});
			}
			s_log.Info("Migration complete. Deleting old data.");

			File.Delete(oldJsonPath);
			return true;
		}

		s_log.Info("Migration not required, continuing with loading.");
		return false;
	}
}