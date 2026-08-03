using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Typeless.Weapon.Support;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.MiscDate;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Typeless.Support
{
    public class YanmeisKnife : CITypeless
    {
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 44;
            Item.damage = 8;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = Item.useTime = 32;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 4.5f;
            Item.autoReuse = false;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item71;
            Item.shoot = ProjectileType<YanmeisKnifeSlash>();
            Item.shootSpeed = 24f;
        }

        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 6;

        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(ItemID.PsychoKnife).
                    AddIngredient(ItemID.Obsidian, 10).
                    AddRecipeGroup(VanillaRecipeGroups.IronBar, 20).
                    AddIngredient(CalamityMaterials.PlagueCellCanister, 50).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
            else
            {

            }
        }
    }
}
