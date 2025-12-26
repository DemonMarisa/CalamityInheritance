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
using System.Collections.Generic;
using System;
using CalamityInheritance.Common.EventChange;
using CalamityInheritance.Buffs.StatDebuffs;
using CalamityInheritance.UI.MusicUI;

namespace CalamityInheritance
{
    // Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
    public class CalamityInheritance : Mod
    {
        public static Mod Calamity => ModLoader.GetMod("CalamityMod");

        public static CalamityInheritance Instance;

        public static readonly BindingFlags UniversalBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        internal Mod FuckYouFargo = null;

        internal Mod infernumMode = null;
        // 获取boss列表
        internal Mod bossChecklist = null;
        //Goozma
        internal Mod Goozma = null;
        //众神
        public static Mod WrathoftheGods = null;
        // 获取灾厄音乐
        internal Mod musicMod = null;
        internal bool MusicAvailable => musicMod is not null;
        // 获取莉莉音乐包
        internal Mod liliesmusicMod = null;

        internal Mod UCA = null;
        #region Mod Support
        public override object Call(params object[] args) => CIModCall.Call(args);
        #endregion
        public override void Load()
        {
            Instance = this;

            UCA = null;
            ModLoader.TryGetMod("UCA", out UCA);
            // 获取灾厄音乐
            musicMod = null;
            ModLoader.TryGetMod("CalamityModMusic", out musicMod);
            // 获取boss列表
            bossChecklist = null;
            ModLoader.TryGetMod("BossChecklist", out bossChecklist);
            // 获取莉莉音乐包
            liliesmusicMod = null;
            ModLoader.TryGetMod("EnderLiliesMusicPack", out liliesmusicMod);
            //goozma
            Goozma = null;
            ModLoader.TryGetMod("CalamityHunt", out Goozma);
            //神怒
            WrathoftheGods = null;
            ModLoader.TryGetMod("NoxusBoss", out WrathoftheGods);
            //fargo
            FuckYouFargo = null;
            CIPlayerDashManager.Load();
            CalamityInheritanceLists.LoadLists();
            /*
            //我没有测试过这个数组能不能用，而且出于某些原因我的游戏又被飞行钩子给干掉了，所以你自己看着办吧（
            DumbMods =
            [
                "CalamityModMusic",
                "BossChecklist",
                "EnderLiliesMusicPack",
                "CalamityHunt",
                "NoxusBoss",
                "Fargowiltas"
            ];
            FuckTheseMod = new Dictionary<string, bool>();
            foreach (string FuckMod in DumbMods)
                FuckTheseMod.Add(FuckMod, false);
            */
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
            if (CalamityLists.debuffList != null)
            {
                CalamityLists.debuffList.Add(ModContent.BuffType<AbyssalFlames>());
                CalamityLists.debuffList.Add(ModContent.BuffType<Horror>());
                CalamityLists.debuffList.Add(ModContent.BuffType<MaliceModeCold>());
                CalamityLists.debuffList.Add(ModContent.BuffType<MaliceModeHot>());
                CalamityLists.debuffList.Add(ModContent.BuffType<VulnerabilityHexLegacy>());
            }
            CIResprite.LoadTexture();
            CIWeaponsResprite.LoadTexture();
            #region Hook
            HeavenlyGaleProjHook.Load(this);
            // 日掉原灾归元的发光贴图
            FuckSubsumingGlowMask.Load(this);
            DOGHook.Load(this);
            // 草捏妈傻逼灾厄飞行条，谁jb判的和坐骑相关啊，似了一万个妈是吧这么判
            // FlightBarDrawHook.Load();
            // 干掉伊布法杖中的变性药水
            // 修复星火bug的hook
            PhotovisceratorCalHook.Load();
            // 邪染特判补全
            ElementalExcaliburTaintedDamageMultHook.Load();
            #endregion
        }
        public void LoadClient()
        {
            // 加载屏幕shader到ModSystem中
            // 红色
            Filters.Scene["CalamityInheritance:SupremeCalamitasLegacy1"] = new Filter(new SCalScreenShaderDataLegacy("FilterMiniTower").UseColor(1.1f, 0.3f, 0.3f).UseOpacity(0.65f), (EffectPriority)10);
            SkyManager.Instance["CalamityInheritance:SupremeCalamitasLegacy1"] = new SCalSkyLegacy();
            // 蓝色
            Filters.Scene["CalamityInheritance:SupremeCalamitasLegacy2"] = new Filter(new SCalScreenShaderDataLegacy("FilterMiniTower").UseColor(0.2f, 0.8f, 1f).UseOpacity(0.5f), (EffectPriority)10);
            SkyManager.Instance["CalamityInheritance:SupremeCalamitasLegacy2"] = new SCalSkyLegacy();
            // 橙色
            Filters.Scene["CalamityInheritance:SupremeCalamitasLegacy3"] = new Filter(new SCalScreenShaderDataLegacy("FilterMiniTower").UseColor(1.1f, 0.4f, 0f).UseOpacity(0.65f), (EffectPriority)10);
            SkyManager.Instance["CalamityInheritance:SupremeCalamitasLegacy3"] = new SCalSkyLegacy();
            //灰色
            Filters.Scene["CalamityInheritance:SupremeCalamitasLegacy4"] = new Filter(new SCalScreenShaderDataLegacy("FilterMiniTower").UseColor(0f, 0f, 0f).UseOpacity(1f), (EffectPriority)10);
            SkyManager.Instance["CalamityInheritance:SupremeCalamitasLegacy4"] = new SCalSkyLegacy();
            // 丛林龙的SKY
            Filters.Scene["CalamityInheritance:Yharon"] = new Filter(new YharonScreenShaderDataLegacy("FilterMiniTower").UseColor(1f, 0.4f, 0f).UseOpacity(0.75f), (EffectPriority)10);
            SkyManager.Instance["CalamityInheritance:Yharon"] = new YharonSkyLegacy();

            // 红色
            Filters.Scene["CalamityInheritance:CalClone"] = new Filter(new SCalScreenShaderDataLegacy("FilterMiniTower").UseColor(1.1f, 0.3f, 0.3f).UseOpacity(0.65f), (EffectPriority)10);
            SkyManager.Instance["CalamityInheritance:CalClone"] = new CalCloneSky();

            AstralArcanumUI.Load(this);
            DifficultyModeUI.Load();
            MusicChoiceUI.Load();
        }

        #region Unload
        public override void Unload()
        {
            UCA = null;
            // 卸载灾厄音乐
            musicMod = null;
            // 卸载莉莉音乐包
            liliesmusicMod = null;
            // 卸载boss列表
            bossChecklist = null;
            //goozma
            Goozma = null;
            //wotg
            WrathoftheGods = null;
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
            /*
            DumbMods = null;
            FuckTheseMod = null;
            */
            DifficultyModeUI.Unload();
            MusicChoiceUI.Unload();
            base.Unload();
        }
        #endregion

        public int? GetMusicFromMusicMod(string songFilename) => MusicAvailable ? MusicLoader.GetMusicSlot(musicMod, "Sounds/Music/" + songFilename) : null;

        #region Mod Support
        public override void PostSetupContent()
        {
            CIWeakReferenceSupport.Setup();

            /*
            //尝试给每个mod加载
            try
            {
                foreach (string ThisMod in DumbMods)
                {
                    FuckTheseMod[ThisMod] = ModLoader.TryGetMod(ThisMod, out Mod getMod);
                }
            }
            //报错输出
            catch (Exception e)
            {
                Logger.Error("CalamityInheritance PostSetupContent Error: " + e.StackTrace + e.Message);
            }
            */
        }
        #endregion

    }
}
