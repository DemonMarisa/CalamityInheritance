
global using static Terraria.ModLoader.ModContent;
using CalamityInheritance.Buffs.StatDebuffs;
using CalamityInheritance.CIPlayer.Dash;
using CalamityInheritance.Common.CIHook;
using CalamityInheritance.Common.ModSupport;
using CalamityInheritance.Content.Items.Weapons.ExoLoreChange;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.NPCs.Boss.CalamitasClone.Sky;
using CalamityInheritance.NPCs.Boss.SCAL.Sky;
using CalamityInheritance.NPCs.Boss.Yharon.Sky;
using CalamityInheritance.UI;
using CalamityInheritance.UI.MusicUI;
using LAP.Core.IDSets;
using LAP.Core.MiscDate;
using System.Reflection;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

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

            LAPIDSet.ProtectedProj.Add(ProjectileType<MurasamaSlashold>());
            LAPIDSet.ProtectedProj.Add(ProjectileType<ExoArrowTealExoLore>());
            LAPIDSet.ProtectedProj.Add(ProjectileType<RogueFallenHammerProjClone>());
            LAPIDSet.ProtectedProj.Add(ProjectileType<RogueFallenHammerProj>());

            if (LAPList.debuffList != null)
            {
                LAPList.debuffList.Add(BuffType<AbyssalFlames>());
                LAPList.debuffList.Add(BuffType<Horror>());
                LAPList.debuffList.Add(BuffType<MaliceModeCold>());
                LAPList.debuffList.Add(BuffType<MaliceModeHot>());
                LAPList.debuffList.Add(BuffType<VulnerabilityHexLegacy>());
            }
            #region Hook
            HeavenlyGaleProjHook.Load(this);
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
            AstralArcanumUI.Unload();
            CalamityInheritanceLists.UnloadLists();
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
