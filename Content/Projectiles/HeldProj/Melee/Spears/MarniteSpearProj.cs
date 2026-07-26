using CalamityInheritance.Content.BaseClass.Projectiles.HeldProj;
using CalamityInheritance.Content.Items.Weapons.Melee.Spear;
using LAP.Core.Utilities;
using Terraria.Localization;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Melee.Spears
{
    public class MarniteSpearProj : BaseSpear
    {
        public override string Texture => GetInstance<MarniteSpear>().Texture;
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<MarniteSpear>();
        public override float RangeMin => 16;
        public override float RangeMax => 62;
    }
}
