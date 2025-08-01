using Microsoft.Xna.Framework;
using System.ComponentModel;
using System.Numerics;
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

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        public bool TurnOffFirstText { get; set; }

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
        public bool TurnoffCorner { get; set; }

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
        public bool ReduceMoveSpeed { get; set; } //是否允许弑神蠕虫

        [BackgroundColor(192, 54, 64, 192)]
        [Range(0, 1)]
        [DefaultValue(1f)]
        public float ReduceMoveSpeedMult{ get; set;}

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        public bool DrawDifficultyUI { get; set; }

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

        
        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(-1000, 1000)]
        [DefaultValue(1)]
        public int Debugint { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(-1000, 1000)]
        [DefaultValue(1)]
        public int Debugint2 { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        public Color DebugColor { get; set; }


        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(-1, 1)]
        [DefaultValue(1)]
        public float Debugfloat { get; set; }
    }
}
