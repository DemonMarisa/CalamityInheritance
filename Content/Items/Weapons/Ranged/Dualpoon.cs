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
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityMod;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Placeables.SunkenSea;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class Dualpoon : CIRanged, ILocalizedModType
    {
        public override void SetDefaults()
        {
            Item.damage = 70;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 56;
            Item.height = 28;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 6.5f;
            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item10;
            Item.autoReuse = true;
            Item.shootSpeed = 20f;
            Item.shoot = ModContent.ProjectileType<DualpoonProj>();
            
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 sourcePlayer = player.RotatedRelativePoint(player.MountedCenter, true);
            float piOverTen = MathHelper.Pi * 0.1f;
            int projCount = 2;
            bool canHit = Collision.CanHit(sourcePlayer, 0, 0, sourcePlayer + velocity, 0, 0);
            for (int projIndex = 0; projIndex < projCount; projIndex++)
            {
                float num120 = projIndex - (projCount - 1f) / 2f;
                Vector2 offset = velocity.RotatedBy((double)(piOverTen * num120), default);
                if (!canHit)
                {
                    offset -= velocity;
                }
                Projectile.NewProjectile(source, sourcePlayer + offset, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Harpoon, 2).
                AddIngredient<SeaPrism>(15).
                AddIngredient(ItemID.SoulofMight, 10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
