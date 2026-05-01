using System.ComponentModel;
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

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool CalBossesCanDropSoul{ get; set; } //允许灾三王掉魂

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool CalExtraDrop { get; set; } //允许灾厄额外掉落

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        public bool CalStatInflationBACK { get; set; } //灾厄数据膨胀回来了

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        [ReloadRequired]
        public bool CalSIBEOtherMod { get; set; }// 是否可以影响其它mod的武器

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        public bool SolarEclipseChange { get; set; } //  龙一龙二

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        public bool LoreDrop{ get; set; } //Lore的掉落方法

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        public bool WeatherChange { get; set; } //环境改变

        [BackgroundColor(192, 54, 64, 192)]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool FuckYouLGBT { get; set; } //🤗

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        public bool TurnOffLevelSystem { get; set; } // 关闭等级系统


        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        public bool MarkSameBossDown { get; set; } // 同类boss击败一次即可
    }
}