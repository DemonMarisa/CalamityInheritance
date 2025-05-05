using CalamityMod;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.CIPlayer.Dash;
using System.Reflection;
using CalamityInheritance.UI;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Texture;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Content.Projectiles.Rogue;
using Terraria.Graphics.Effects;
using CalamityInheritance.NPCs.Boss.SCAL.Sky;
using CalamityInheritance.Content.Items.Weapons.ExoLoreChange;
using CalamityInheritance.Common.ModSupport;
using CalamityInheritance.NPCs.Boss.Yharon.Sky;
using CalamityInheritance.Common.CIHook;
using CalamityInheritance.NPCs.Boss.CalamitasClone.Sky;
using Terraria.ModLoader.Config;

namespace CalamityInheritance
{
    // Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
    public class CalamityInheritance : Mod
    {
        public static Mod Calamity => ModLoader.GetMod("CalamityMod");

        public static CalamityInheritance Instance;

        public static readonly BindingFlags UniversalBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        internal Mod infernumMode = null;
        // 获取boss列表
        internal Mod bossChecklist = null;
        // 获取灾厄音乐
        internal Mod musicMod = null;
        internal bool MusicAvailable => musicMod is not null;
        // 获取莉莉音乐包
        internal Mod liliesmusicMod = null;

        public override void Load()
        {
            Instance = this;

            // 获取灾厄音乐
            musicMod = null;
            ModLoader.TryGetMod("CalamityModMusic", out musicMod);
            // 获取boss列表
            bossChecklist = null;
            ModLoader.TryGetMod("BossChecklist", out bossChecklist);
            // 获取莉莉音乐包
            liliesmusicMod = null;
            ModLoader.TryGetMod("EnderLiliesMusicPack", out liliesmusicMod);

            CIPlayerDashManager.Load();
            CalamityInheritanceLists.LoadLists();

            if (!Main.dedServ)
            {
                LoadClient();
            }

            infernumMode = null;
            ModLoader.TryGetMod("InfernumMode", out infernumMode);

            if (CalamityLists.pierceResistExceptionList != null)
            {
                CalamityLists.pierceResistExceptionList.Add(ModContent.ProjectileType<MurasamaSlashnew1>());
                CalamityLists.pierceResistExceptionList.Add(ModContent.ProjectileType<MurasamaSlashold>());
                CalamityLists.pierceResistExceptionList.Add(ModContent.ProjectileType<ExoArrowTealExoLore>());
                CalamityLists.pierceResistExceptionList.Add(ModContent.ProjectileType<DragonBowFlameRework>());
                CalamityLists.pierceResistExceptionList.Add(ModContent.ProjectileType<RogueTypeHammerTruePaladinsProjClone>());
                CalamityLists.pierceResistExceptionList.Add(ModContent.ProjectileType<RogueTypeHammerTruePaladinsProj>());
            }
            if (CalamityLists.projectileDestroyExceptionList != null)
            {
                CalamityLists.projectileDestroyExceptionList.Add(ModContent.ProjectileType<MurasamaSlashnew1>());
                CalamityLists.projectileDestroyExceptionList.Add(ModContent.ProjectileType<MurasamaSlashold>());
                CalamityLists.projectileDestroyExceptionList.Add(ModContent.ProjectileType<ExoArrowTealExoLore>());
                CalamityLists.projectileDestroyExceptionList.Add(ModContent.ProjectileType<RogueTypeHammerTruePaladinsProjClone>());
                CalamityLists.projectileDestroyExceptionList.Add(ModContent.ProjectileType<RogueTypeHammerTruePaladinsProj>());
            }
            CIResprite.LoadTexture();
            CIWeaponsResprite.LoadTexture();
            #region Hook
            CalamityInheritanceDashHook.Load(this);
            HeavenlyGaleProjHook.Load(this);
            //日掉原灾归元的发光贴图
            FuckSubsumingGlowMask.Load(this);
            DOGHook.Load(this);
            FlightBarDrawHook.Load();
            DropHelperHook.Load(this);
            #endregion
        }
        public void LoadClient()
        {
            // 加载屏幕shader到ModSystem中
            // 红色
            Filters.Scene["CalamityInheritance:SupremeCalamitasLegacy1"] = new Filter(new SCalScreenShaderDataLegacy("FilterMiniTower").UseColor(1.1f, 0.3f, 0.3f).UseOpacity(0.65f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalamityInheritance:SupremeCalamitasLegacy1"] = new SCalSkyLegacy();
            // 蓝色
            Filters.Scene["CalamityInheritance:SupremeCalamitasLegacy2"] = new Filter(new SCalScreenShaderDataLegacy("FilterMiniTower").UseColor(0.2f, 0.8f, 1f).UseOpacity(0.5f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalamityInheritance:SupremeCalamitasLegacy2"] = new SCalSkyLegacy();
            // 橙色
            Filters.Scene["CalamityInheritance:SupremeCalamitasLegacy3"] = new Filter(new SCalScreenShaderDataLegacy("FilterMiniTower").UseColor(1.1f, 0.4f, 0f).UseOpacity(0.65f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalamityInheritance:SupremeCalamitasLegacy3"] = new SCalSkyLegacy();
            //灰色
            Filters.Scene["CalamityInheritance:SupremeCalamitasLegacy4"] = new Filter(new SCalScreenShaderDataLegacy("FilterMiniTower").UseColor(0f, 0f, 0f).UseOpacity(1f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalamityInheritance:SupremeCalamitasLegacy4"] = new SCalSkyLegacy();
            // 丛林龙的SKY
            Filters.Scene["CalamityInheritance:Yharon"] = new Filter(new YharonScreenShaderDataLegacy("FilterMiniTower").UseColor(1f, 0.4f, 0f).UseOpacity(0.75f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalamityInheritance:Yharon"] = new YharonSkyLegacy();

            // 红色
            Filters.Scene["CalamityInheritance:CalClone"] = new Filter(new SCalScreenShaderDataLegacy("FilterMiniTower").UseColor(1.1f, 0.3f, 0.3f).UseOpacity(0.65f), EffectPriority.VeryHigh);
            SkyManager.Instance["CalamityInheritance:CalClone"] = new CalCloneSky();

            AstralArcanumUI.Load(this);
            CalPopupGUIManager.LoadGUIs();
            DifficultyModeUI.Load();
        }

        #region Unload
        public override void Unload()
        {
            // 卸载灾厄音乐
            musicMod = null;
            // 卸载莉莉音乐包
            liliesmusicMod = null;
            // 卸载boss列表
            bossChecklist = null;
            CIPlayerDashManager.Unload();
            AstralArcanumUI.Unload();
            CalamityInheritanceLists.UnloadLists();
            if (CalamityLists.pierceResistExceptionList != null)
            {
                CalamityLists.pierceResistExceptionList.Remove(ModContent.ProjectileType<MurasamaSlashnew1>());
                CalamityLists.pierceResistExceptionList.Add(ModContent.ProjectileType<MurasamaSlashold>());
            }
            CIResprite.UnloadTexture();
            CIWeaponsResprite.UnloadTexture();
            infernumMode = null;
            Instance = null;

            DifficultyModeUI.Unload();
            CalPopupGUIManager.UnloadGUIs();
            base.Unload();
        }
        #endregion

        public int? GetMusicFromMusicMod(string songFilename) => MusicAvailable ? MusicLoader.GetMusicSlot(musicMod, "Sounds/Music/" + songFilename) : null;

        #region Mod Support
        public override void PostSetupContent() => CIWeakReferenceSupport.Setup();
        #endregion

        #region Force ModConfig save (Reflection)
        internal static void SaveConfig(CalamityConfig cfg)
        {
            // There is no current way to manually save a mod configuration file in tModLoader.
            // The method which saves mod config files is private in ConfigManager, so reflection is used to invoke it.
            try
            {
                MethodInfo saveMethodInfo = typeof(ConfigManager).GetMethod("Save", BindingFlags.Static | BindingFlags.NonPublic);
                if (saveMethodInfo is not null)
                    saveMethodInfo.Invoke(null, new object[] { cfg });
                else
                    Instance.Logger.Error("TML ConfigManager.Save reflection failed. Method signature has changed. Notify Calamity Devs if you see this in your log.");
            }
            catch
            {
                Instance.Logger.Error("An error occurred while manually saving Calamity mod configuration. This may be due to a complex mod conflict. It is safe to ignore this error.");
            }
        }
        #endregion
    }
}
