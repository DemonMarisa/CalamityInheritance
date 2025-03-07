using System.ComponentModel;
using System.Net;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ModLoader.Config;

namespace CalamityInheritance.System.Configs
{

    [BackgroundColor(49, 32, 36, 216)]
    public class CIServerConfig : ModConfig
    {
        public static CIServerConfig Instance;
        public override void OnLoaded()
        {
            Instance = this;
        }
        public override ConfigScope Mode => ConfigScope.ServerSide;
        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref NetworkText message) => false;

        [Header("GeneralGamePlay")]
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        [ReloadRequired]
        public bool LegendaryitemsRecipes { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool CustomShimmer{ get; set; } //微光嬗变
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        [ReloadRequired]
        public bool VanillaUnnerf{ get; set; } //原版数值回调
    }
}