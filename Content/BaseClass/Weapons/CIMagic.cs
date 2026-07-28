using CalamityInheritance.Core.Path;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass.Weapons
{
    public abstract class CIMagic : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => LocalizationPath.MagicWeapon;
        public override void SetDefaults()
        {
            Item.width = Item.height = 32;
            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;
            Item.autoReuse = true;
            ExSD();
        }
        public virtual void ExSD()
        {
        }
    }
}
