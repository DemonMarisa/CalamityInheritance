using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Weapons.Ranged;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class PaintballBlaster : CIRanged, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Type.ShimmerEach<SpeedBlaster>();
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 26;
            Item.damage = 26;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 4;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = CIShopValue.RarityPricePink;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 24;
            Item.knockBack = 2.25f;
            Item.shootSpeed = 26f;
            Item.shoot = ProjectileID.PainterPaintball;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-6, -6);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 Xoffset = new Vector2(20, 0).RotatedBy(velocity.ToRotation());
            Vector2 Yoffset = new Vector2(0, -11);
            Vector2 finaloffset = Xoffset + Yoffset;
            Projectile.NewProjectile(source,position + finaloffset, velocity.RotatedByRandom(MathHelper.ToRadians(5)) * Main.rand.NextFloat(0.9f, 1.1f), ProjectileID.PainterPaintball, damage, knockback, player.whoAmI, 0f, Main.rand.Next(12) / 6f);
            return false;
        }

        public override void UseItemFrame(Player player)
        {
            CIFunction.NoHeldProjUpdateAim(player, 0, 1);
            float rotation = (player.Center - player.LocalMouseWorld()).ToRotation() * player.gravDir + MathHelper.PiOver2;
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rotation);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.PainterPaintballGun).
                AddIngredient(ItemID.SoulofSight, 5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}