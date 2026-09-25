using CalamityInheritance.Core.Utils;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using Terraria.ModLoader;

namespace CalamityInheritance.Common.CalamityModCross
{
    public class CalDeBuff : ModSystem
    {
        public static int AstralInfectionDebuff;
        public static int Irradiated;
        public static int BrainRot;
        public static int BurningBlood;
        public static int BrimstoneFlames;
        public static int Plague;
        public static int ArmorCrunch;
        public static int RiptideDebuff;
        public static int CrushDepth;
        public static int SulphuricPoisoning;
        public static int GodSlayerInferno;
        public static int HolyFlames;
        public static int IcarusFolly;
        public override void Load()
        {
            if (CIUtils.HasCalamity())
            {
                GetCalBuffID();
            }
        }
        [JITWhenModsEnabled("CalamityMod")]
        public static void GetCalBuffID()
        {
            AstralInfectionDebuff = BuffType<AstralInfectionDebuff>();
            Irradiated = BuffType<Irradiated>();
            BrainRot = BuffType<BrainRot>();
            BurningBlood = BuffType<BurningBlood>();
            BrimstoneFlames = BuffType<BrimstoneFlames>();
            Plague = BuffType<Plague>();
            ArmorCrunch = BuffType<ArmorCrunch>();
            RiptideDebuff = BuffType<RiptideDebuff>();
            CrushDepth = BuffType<CrushDepth>();
            SulphuricPoisoning = BuffType<SulphuricPoisoning>();
            GodSlayerInferno = BuffType<GodSlayerInferno>();
            HolyFlames = BuffType<HolyFlames>();
            IcarusFolly = BuffType<IcarusFolly>();
        }
    }
}
