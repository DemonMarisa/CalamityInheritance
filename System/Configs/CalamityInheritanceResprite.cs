using System.ComponentModel;
using Terraria.Localization;
using Terraria.ModLoader.Config;

namespace CalamityInheritance.System.Configs
{
    [BackgroundColor(49, 32, 36, 216)]
    public class CIRespriteConfig : ModConfig
    {
        public static CIRespriteConfig Instance;
        public override void OnLoaded()
        {
            Instance = this;
        }
        public override ConfigScope Mode => ConfigScope.ClientSide;
        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref NetworkText message) => false;

        [Header("Texture")]
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        public bool WulfumTexture { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        public bool SetAllLegacySprite{ get; set; }

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
        [Range(1, 4)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]
        public int ShadowspecKnivesResprite{ get; set; }
        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 2)]
        [Increment(1)]
        public int TriactisHammerResprite{ get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 2)]
        [Increment(1)]
        public int PwnagehammerResprite{ get; set; }
        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 2)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]

        public int FateGirlSprite{ get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 2)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]
        public int StellarContemptResprite { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 2)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]
        public int HeliumFlashResprite{ get; set; }
        
        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 2)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]
        public int DrataliornusResprite { get; set; }

        #region 材料/物品贴图
        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 2)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]
        public int BloodOrangeResprite{ get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 2)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]
        public int MiracleFruitResprite{ get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 2)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]
        public int ElderberryResprite{ get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 2)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]
        public int DragonfruitResprite{ get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 2)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]
        public int CometShardResprite{ get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 2)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]
        public int EtherealCoreResprite{ get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 2)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]
        public int PhantomHeartResprite{ get; set; }
        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 2)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]
        public int AngelTreadsResprite{ get; set; }
        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 2)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]
        public int LunarBootsResprite{ get; set; }
        [BackgroundColor(192, 54, 64, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 2)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]
        public int MOABResprite{ get; set; }
        #endregion
    }
}