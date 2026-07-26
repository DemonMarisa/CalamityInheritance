using CalamityInheritance.Core.Path;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass.Weapons
{
    public abstract class CIRanged : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => LocalizationPath.RangedWeapon;
        public override void SetDefaults()
        {
            Item.width = Item.height = 32;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            ExSD();
        }
        public virtual void ExSD()
        {
        }
    }
}
