using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Ranged.HandGun;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.HandGun
{
    public class ClaretCannonLegacy : CIRanged
    {
        public override void ExSD()
        {
            Item.damage = 140;
            Item.useTime = Item.useAnimation = 19;
            Item.knockBack = 5.5f;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<AbsoluteGreen>();
            Item.UseSound = SoundID.Item40;
            Item.shootSpeed = 24;
            Item.shoot = ProjectileType<ClaretCannonBullet>();
        }
        public override Vector2? HoldoutOffset() => new Vector2(-5, 0);
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.BloodstoneCore, 4).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
            else
            {
                CreateRecipe().
                        AddIngredient(ItemID.LunarBar, 4).
                        AddTile(TileID.LunarCraftingStation).
                        Register();
            }
        }
        public override void UseItemFrame(Player player)
        {
            player.ChangeDir(Math.Sign((player.LocalMouseWorld() - player.Center).X));

            float animProgress = 0.5f - player.itemTime / (float)player.itemTimeMax;
            // 向鼠标的旋转
            float rotation = (player.Center - player.LocalMouseWorld()).ToRotation() * player.gravDir + MathHelper.PiOver2;
            float offset = -0.03f * (float)Math.Pow((0.6f - animProgress) / 0.6f, 2);
            if (animProgress < 0.4f)
                rotation += offset * player.direction;

            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rotation);
            CIUtils.NoHeldProjUpdateAim(player, MathHelper.ToDegrees(offset), 1);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 3; i++)
            {
                Projectile.NewProjectile(source, position - Vector2.UnitY * 10, velocity.RotatedByRandom(0.02f) * Main.rand.NextFloat(0.8f, 1.1f), type, damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
}
