using CalamityInheritance.Content.Projectiles.Rogue.Boomerang;
using CalamityInheritance.Rarity;
using CalamityInheritance.Texture;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Rogue.Boomerang
{
    public class ToxicantTwisterLegacy : CIRogueClass
    {
        public override void ExSD()
        {
            Item.width = 42;
            Item.height = 46;
            Item.damage = 333;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<ToxicantTwisterProjLegacy>();
            Item.shootSpeed = 18f;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<AbsoluteGreen>();
        }

        public override float StealthDamageMultiplier => 1.3f;
    }
}
