using CalamityInheritance.Core.Utils;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using Terraria.ModLoader;

namespace CalamityInheritance.Common.CalamityModCross
{
    public class CalamityWeapons : ModSystem
    {
        public static int PestilentDefiler;
        public static int SolsticeClaymore;
        public static int ValkyrieRay;
        public override void OnModLoad()
        {
            if (CIUtils.HasCalamity())
            {
                GetCalamityWeaponID();
            }
        }
        [JITWhenModsEnabled("CalamityMod")]
        public static void GetCalamityWeaponID()
        {
            PestilentDefiler = ItemType<PestilentDefiler>();
            SolsticeClaymore = ItemType<SolsticeClaymore>();
            ValkyrieRay = ItemType<ValkyrieRay>();
        }
    }
}
