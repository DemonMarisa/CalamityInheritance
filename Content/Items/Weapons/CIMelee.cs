using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons
{
    public abstract class CIMelee: ModItem
    {
        public new string LocalizationCategory => $"{Generic.BaseWeaponCategory}.Melee";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            base.SetStaticDefaults();
        }
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.MeleeWeapon;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            CIFunction.BetterSwing(player);
        }
    }
}