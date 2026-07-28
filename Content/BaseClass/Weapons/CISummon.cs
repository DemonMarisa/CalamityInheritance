using CalamityInheritance.Core.Path;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass.Weapons
{
    public abstract class CISummmon : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => LocalizationPath.SummonWeapon;
        public override void SetDefaults()
        {
            Item.width = Item.height = 32;
            Item.DamageType = DamageClass.Summon;
            Item.noMelee = true;
            Item.autoReuse = true;
            ExSD();
        }
        public virtual void ExSD()
        {
        }
    }
}
