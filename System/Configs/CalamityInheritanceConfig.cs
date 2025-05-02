using System.ComponentModel;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace CalamityInheritance.System.Configs
{
    [BackgroundColor(49, 32, 36, 216)]
    public class CIConfig : ModConfig
    {
        public static CIConfig Instance;
        public override ConfigScope Mode => ConfigScope.ClientSide;
        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref NetworkText message) => true;

        #region General Gameplay Changes

        [Header("Gameplay")]
       
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        public bool ElementalQuiversplit { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 4)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]
        [Slider]
        public int ElementalQuiverSplitstyle { get; set; }
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        public bool TheSpongeBarrier { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        public bool BoCLoreUnconditional { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 3)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]
        [Slider]
        public int GodSlayerSetBonusesChange { get; set; }
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        public bool silvastun { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        public bool ExoSperaHitEffect { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        public bool AmmoConversion { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        public bool turnoffCorner { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        [ReloadRequired]
        public bool SpecialRarityColor{ get; set; } //部分物品的特殊颜色

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        [ReloadRequired]
        public bool LegendaryRarity{ get; set; } //传奇武器特殊稀有度
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        public bool GodSlayerWorm{ get; set;} //是否允许弑神蠕虫
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        public bool ReduceMoveSpeed{ get; set;}

        #endregion
        [Header("Music")]
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        public bool TaleOfACruelWorld { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        public bool BlessingoftheMoon { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        public bool Tyrant1 { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        public bool Exomechs { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        public bool Scal { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        public bool DoGLegacyMusic { get; set; }
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        public bool Arcueid{ get; set; }
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        public bool Kunoji{ get; set; }
    }
}
