using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.HeldProj.Ranged.HandGun;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.HandGun
{
    public class PearlGodLegacy : CIRanged
    {
        public static int damage = 110;
        public override void SetDefaults()
        {
            Item.width = 80;
            Item.height = 46;
            Item.damage = damage;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;
            Item.shoot = ProjectileType<PearlGodLegacyHeldProj>();
            Item.useAmmo = AmmoID.Bullet;

            Item.scale = 0.75f;

            Item.noUseGraphic = true;
            Item.channel = true;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ProjectileType<PearlGodLegacyHeldProj>()] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity * 0.1f, ProjectileType<PearlGodLegacyHeldProj>(), damage, knockback, player.whoAmI, 0f, 0f, 0f);
            return false;
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient<AeriesLegacy>().
                    AddIngredient(CalamityMaterials.LifeAlloy, 5).
                    AddIngredient(CalamityMaterials.RuinousSoul, 5).
                    AddIngredient(ItemID.WhitePearl).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddIngredient<AeriesLegacy>().
                    AddIngredient<GalacticaSingularity>(5).
                    AddIngredient(ItemID.WhitePearl).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
        }
    }
}
