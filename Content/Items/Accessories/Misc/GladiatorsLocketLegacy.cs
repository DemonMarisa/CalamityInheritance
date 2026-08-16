using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.SummonBuff.Accessories;
using CalamityInheritance.Content.Projectiles.Typeless.Accessories;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Misc
{
    public class GladiatorsLocketLegacy : CIAccessories
    {
        public override int AccessoriesStyle => Misc;
        public override void SetDefaults()
        {
            Item.height = 36;
            Item.width = 42;
            Item.rare = ItemRarityID.Orange;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (player.FindBuffIndex(BuffType<GladiatorsLocketLegacyBuff>()) == -1)
                {
                    player.AddBuff(BuffType<GladiatorsLocketLegacyBuff>(), 2, true);
                }
                if (player.ownedProjectileCounts[ProjectileType<ShrineMarbleSword>()] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ProjectileType<ShrineMarbleSword>(), (int)player.GetTotalDamage<GenericDamageClass>().ApplyTo(50), 6f, Main.myPlayer, 1f);
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ProjectileType<ShrineMarbleSword>(), (int)player.GetTotalDamage<GenericDamageClass>().ApplyTo(50), 6f, Main.myPlayer);
                }
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Marble, 15);
            recipe.Register();
        }
    }
}