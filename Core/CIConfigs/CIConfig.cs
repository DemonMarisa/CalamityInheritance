using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria.ModLoader.Config;

namespace CalamityInheritance.Core.CIConfigs
{
    public class CIConfig : ModConfig
    {
        public static CIConfig Instance;
        public override ConfigScope Mode => ConfigScope.ClientSide;
        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref NetworkText message) => true;

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        public bool TurnoffCorner { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        public bool DrawDifficultyUI { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        public bool CalClone { get; set; }
    }
}
