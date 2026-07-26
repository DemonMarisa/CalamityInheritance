using CalamityInheritance.Content.BaseClass.Projectiles.HeldProj;
using CalamityInheritance.Content.Items.Weapons.Melee.Spear;
using LAP.Core.Utilities;
using Terraria.Localization;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Melee.Spears
{
    public class FulgurationHalberdProj : BaseSpear
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<FulgurationHalberd>();
        public override float RangeMin => 16;
        public override float RangeMax => 76;
    }
}
