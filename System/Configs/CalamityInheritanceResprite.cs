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
        

        [BackgroundColor(192, 154, 205, 192)]
        [DefaultValue(false)]
        public bool SetAllLegacySprite{ get; set; }

        [Header("Weapons")]
        //战士
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool ArkofCosmosTexture { get; set; }

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool AegisResprite { get; set;}

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool CrescentMoonResprite { get; set;}

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool GalileoResprite { get; set;}
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]

        public bool ElementalShivResprite { get ; set ; }

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool CaliburShivResprite { get; set;}

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool TrueCaliburShivResprite { get; set;}

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool NightShivResprite { get; set;}

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool TrueNightShivResprite { get; set;}

        //射手
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool SkullmasherResprite{ get; set; }

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool P90Resprite { get; set; }

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool DrataliornusResprite { get; set; }

        //法师
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool HeliumFlashResprite{ get; set; }
        //战/盗混合
        
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool PwnagehammerResprite{ get; set; }

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool StellarContemptResprite { get; set; }

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool TriactisHammerResprite{ get; set; }

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool GodSlayerKnivesResprite{ get; set; }

        [BackgroundColor(43, 56, 95, 182)]
        [SliderColor(224, 165, 56, 128)]
        [Range(1, 4)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(1)]
        public int ShadowspecKnivesResprite{ get; set; }

        [Header("Accessories")]

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool RampartofDeitiesTexture { get; set; }

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool EtherealTalismancTexture { get; set; }
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool FateGirlSprite{ get; set; }
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool AngelTreadsResprite{ get; set; }
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool LunarBootsResprite{ get; set; }
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool MOABResprite{ get; set; }

        [Header("Misc")]
        #region 材料/物品贴图
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool WulfumTexture { get; set; }
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool BloodOrangeResprite{ get; set; }

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool MiracleFruitResprite{ get; set; }

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool ElderberryResprite{ get; set; }

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool DragonfruitResprite{ get; set; }

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool CometShardResprite{ get; set; }

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool EtherealCoreResprite{ get; set; }

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool PhantomHeartResprite{ get; set; }
        
        
        #endregion
    }
}