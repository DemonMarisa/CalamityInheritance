using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Weapons.Ranged;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class CursedCapper : CIRanged, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Type.ShimmerEach<ThermoclineBlaster>(false);
        }
        public override void SetDefaults()
        {
            Item.damage = 35;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 52;
            Item.height = 32;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2.25f;
            Item.value = CIShopValue.RarityPriceLightRed;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item41;
            Item.autoReuse = true;
            Item.shootSpeed = 14f;
            Item.shoot = ProjectileID.CursedBullet;
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().canFirePointBlankShots = true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, -4);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y - 10, velocity.X, velocity.Y, ProjectileID.CursedBullet, damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.PhoenixBlaster).
                AddIngredient(ItemID.CursedFlame, 10).
                AddTile(TileID.Anvils).
                Register();
        }
        public override void UseItemFrame(Player player)
        {
            player.ChangeDir(Math.Sign((player.Calamity().mouseWorld - player.Center).X));

            float animProgress = 0.5f - player.itemTime / (float)player.itemTimeMax;
            // 向鼠标的旋转
            float rotation = (player.Center - player.Calamity().mouseWorld).ToRotation() * player.gravDir + MathHelper.PiOver2;
            float offset = -0.03f * (float)Math.Pow((0.6f - animProgress) / 0.6f, 2);
            if (animProgress < 0.4f)
                rotation += offset * player.direction;

            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rotation);
            CIFunction.NoHeldProjUpdateAim(player, MathHelper.ToDegrees(offset), 1);
        }
    }
}
