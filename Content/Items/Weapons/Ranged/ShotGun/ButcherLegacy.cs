using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.HeldProj.Ranged.ShotGun;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Content.RecipeGroupAdd;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.ShotGun
{
    public class ButcherLegacy : CIRanged
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 12;
            Item.damage = 15;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 1f;
            Item.value = CIShopValue.RarityPriceLightRed;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item38;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            Item.channel = true;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<ButcherHeldProj>();
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;

        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

        // Spawning the holdout cannot consume ammo
        public override bool CanConsumeAmmo(Item ammo, Player player) => !Main.rand.NextBool(4) && player.ownedProjectileCounts[Item.shoot] > 0;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ProjectileType<ButcherHeldProj>(), damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(ItemID.Shotgun).
                    AddIngredient(ItemID.IllegalGunParts).
                    AddRecipeGroup(LAPRecipeGroup.AnyCobaltBar, 5).
                    AddIngredient(CalamityMaterials.EssenceofHavoc, 4).
                    AddTile(TileID.Anvils).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddIngredient(ItemID.Shotgun).
                    AddIngredient(ItemID.IllegalGunParts).
                    AddRecipeGroup(LAPRecipeGroup.AnyCobaltBar, 5).
                    AddTile(TileID.Anvils).
                    Register();
            }
        }
    }
}
