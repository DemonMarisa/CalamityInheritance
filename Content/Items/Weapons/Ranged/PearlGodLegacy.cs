using CalamityMod.Items;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityMod.Items.Materials;
using CalamityInheritance.Content.Projectiles.HeldProj.Ranged;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class PearlGodLegacy : CIRanged, ILocalizedModType
    {
        public override void SetDefaults()
        {
            Item.width = 80;
            Item.height = 46;
            Item.damage = 120;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 9;
            Item.useAnimation = 9;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<PearlGodLegacyHeldProj>();
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().canFirePointBlankShots = true;
            Item.scale = 0.75f;

            Item.noUseGraphic = true;
            Item.channel = true;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<PearlGodLegacyHeldProj>()] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity * 0.1f, ModContent.ProjectileType<PearlGodLegacyHeldProj>(), damage, knockback, player.whoAmI, 0f, 0f, 0f);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AeriesLegacy>().
                AddIngredient<LifeAlloy>(5).
                AddIngredient<RuinousSoul>(2).
                AddIngredient(ItemID.WhitePearl).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
