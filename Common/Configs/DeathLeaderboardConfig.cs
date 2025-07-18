using System.ComponentModel;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;
using Terraria.ModLoader.Config;

namespace DeathLeaderboard.Common.Configs
{
    internal class DeathLeaderboardConfig : ModConfig
    {
        protected internal const string JsonLabel           = "$Mods.DeathLeaderboard.Common.Configs.StorageType.JsonLabel";
        protected internal const string JsonTooltip         = "$Mods.DeathLeaderboard.Common.Configs.StorageType.JsonTooltip";
        protected internal const string InternalLabel       = "Mods.DeathLeaderboard.Common.Configs.StorageType.InternalLabel";
        protected internal const string InternalTooltip     = "Mods.DeathLeaderboard.Common.Configs.StorageType.InternalTooltip";

        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("Storage Type")]
        [DefaultValue(StorageType.Internal)]
        [JsonConverter(typeof(StringEnumConverter))]
        public static StorageType StorageType { get; set; }
    }

    internal enum StorageType
    {
        [LabelKey(DeathLeaderboardConfig.JsonLabel), TooltipKey(DeathLeaderboardConfig.JsonTooltip)]
        Json = 0,
        [LabelKey(DeathLeaderboardConfig.InternalLabel), TooltipKey(DeathLeaderboardConfig.InternalTooltip)]
        Internal = 1
    }
}