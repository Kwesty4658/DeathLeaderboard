using Terraria;
using Terraria.DataStructures;

namespace DeathLeaderboard.Common
{
    internal static class DeathCauseHelper
    {
        internal static string GetDeathCause(PlayerDeathReason reason, int lastAttackerType)
        {
            if (reason.SourceOtherIndex != -1 && reason.SourceOtherIndex >= 0)
                return GetOtherDeathSystemource(reason);

            else if (reason.SourceNPCIndex != -1)
                return Lang.GetNPCNameValue(lastAttackerType);

            else if (reason.SourcePlayerIndex != -1)
            {
                foreach (var p in Main.ActivePlayers)
                    if (p.whoAmI == reason.SourcePlayerIndex)
                        return p.name;

                return "Unknown";
            }
            else
                return "Unknown";
        }

        private static string GetOtherDeathSystemource(PlayerDeathReason reason)
        {
            return (OtherDeathCause)reason.SourceOtherIndex switch
            {
                OtherDeathCause.Fell => "Falling",
                OtherDeathCause.Drowned => "Drowning",
                OtherDeathCause.Lava => "Lava",
                OtherDeathCause.Unknown => "Unknown",
                OtherDeathCause.Slain => "Combat",
                OtherDeathCause.Petrified => "Petrification",
                OtherDeathCause.Stabbed => "Stabbing",
                OtherDeathCause.Suffocated => "Suffocation",
                OtherDeathCause.Burned => "Burning",
                OtherDeathCause.Poisoned => "Poisoning",
                OtherDeathCause.Electrocuted => "Electrocution",
                OtherDeathCause.AttemptedEscape => "Failed Escape",
                OtherDeathCause.Licked => "Licked to Death",
                OtherDeathCause.Teleport1 => "Teleportation Mishap",
                OtherDeathCause.Teleport2Male => "Teleportation Mishap",
                OtherDeathCause.Teleport2Female => "Teleportation Mishap",
                OtherDeathCause.Inferno => "Inferno",
                OtherDeathCause.DiedInTheDark => "Darkness",
                OtherDeathCause.Starved => "Starvation",
                OtherDeathCause.Space => "Vacuum of Space",
                OtherDeathCause.Empty => "Unknown",
                OtherDeathCause.Slain2 => "Combat",
                _ => "Unknown"
            };
        }
    }
    
    internal enum OtherDeathCause
    {
        Fell = 0,
        Drowned = 1,
        Lava = 2,
        Unknown = 3,
        Slain = 4,
        Petrified = 5,
        Stabbed = 6,
        Suffocated = 7,
        Burned = 8,
        Poisoned = 9,
        Electrocuted = 10,
        AttemptedEscape = 11,
        Licked = 12,
        Teleport1 = 13,
        Teleport2Male = 14,
        Teleport2Female = 15,
        Inferno = 16,
        DiedInTheDark = 17,
        Starved = 18,
        Space = 19,
        Empty = 254,
        Slain2 = 255
    }
}