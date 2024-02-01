using System.Text;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Collections.Concurrent;

namespace DeathLeaderboard
{
    public static class Leaderboard
    {
        public static void DisplayLeaderboard(ConcurrentDictionary<string, int> data)
        {
            //sorts dicitionary by descending values
            var sortedData = from entry in data orderby entry.Value descending select entry;
            
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Death leaderboard:");
            foreach (KeyValuePair<string, int> kvp in sortedData)
            {
                sb.AppendLine($"  {kvp.Key}: {kvp.Value}");
            }

            MessageHandler.SendMessage(sb.ToString(), Color.Red);
            
        }
    }
}