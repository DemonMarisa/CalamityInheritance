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

        [Header("Weapons")]
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool AllElemental{ get; set; }
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool AllShivs{ get; set; }
        //暂时注释掉泰拉系列的贴图切换，因为我尚不清楚如何ban掉发光贴图
        // [BackgroundColor(43, 56, 95, 182)]
        // [DefaultValue(false)]
        // public bool AllTerra{ get; set; }
        //星流
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool FuckAllExo{ get; set; }
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
        public bool MirrorBlade{ get; set;}

        //射手
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool SkullmasherResprite{ get; set; }

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool P90Resprite { get; set; }
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool SomaPrime{ get; set; }

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool DrataliornusResprite { get; set; }

        //法师
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool HeliumFlashResprite{ get; set; }

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool PlasmaRod{ get; set; }

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool StaffofBlushie{ get; set; }
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
        public bool PrismallineResprite{ get; set; }

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool RadiantStarResprite{ get; set; }

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool ShatteredSunResprite{ get; set; }
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool ScarletDevil{ get; set; }
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool BrinyBaronResprite{ get; set; }
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool PlantBowResprite{ get; set; }
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool Vesu{ get; set; }
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool SHPC{ get; set; }
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool Mala{ get; set; }

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
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool FrigidBulwarkResprite { get; set; }
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        public bool FrostBarrierResprite { get; set; }
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