using System.ComponentModel;
using System.Runtime.Serialization;
using CalamityMod;
using CalamityMod.UI;
using CalamityMod.UI.DraedonsArsenal;
using CalamityMod.UI.Rippers;
using CalamityMod.UI.SulphurousWaterMeter;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader.Config;

namespace CalamityInheritance
{
    [BackgroundColor(49, 32, 36, 216)]
    public class CalamityInheritanceConfig : ModConfig
    {
        public static CalamityInheritanceConfig Instance;
        public override ConfigScope Mode => ConfigScope.ClientSide;
        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref NetworkText message) => true;

        #region General Gameplay Changes

        [Header("Gameplay")]
        /*
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        public bool MusrasamaSlashchange { get; set; }
        */
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        public bool ElementalQuiversplit { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 4)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]
        public int ElementalQuiverSplitstyle { get; set; }
        /*
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        public bool Exobladeprojectile { get; set; }
        */
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
        [DefaultValue(false)]
        public bool turnoffCorner { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        [ReloadRequired]
        public bool LegendaryitemsRecipes { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool CustomShimmer{ get; set; } //微光嬗变

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool CalBossesCanDropSoul{ get; set; } //允许灾三王掉魂

        #endregion

        [Header("Texture")]
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        public bool WulfumTexture { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 2)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]
        public int ArkofCosmosTexture { get; set; }


        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 2)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]
        public int RampartofDeitiesTexture { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 2)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]
        public int EtherealTalismancTexture { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 2)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]
        public int SkullmasherResprite{ get; set; }


        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 2)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]
        public int P90Resprite { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 2)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]
        public int GodSlayerKnivesResprite{ get; set; }
        
        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 3)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]
        public int ShadowspecKnivesResprite{ get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 2)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]
        public int FateGirlSprite{ get; set; }

        [Header("Music")]
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
    }
}
