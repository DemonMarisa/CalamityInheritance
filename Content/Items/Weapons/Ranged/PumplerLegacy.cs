using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Weapons.Ranged;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class PumplerLegacy : CIRanged, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Type.ShimmerEach<Pumpler>();
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.damage = 8;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 62;
            Item.height = 36;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.25f;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 11f;
            Item.useAmmo = AmmoID.Bullet;
            
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, -4);
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.autoReuse = true;
                Item.shootSpeed = 4f;
            }
            else
            {
                Item.autoReuse = true;
                Item.shootSpeed = 11f;
            }
            return base.CanUseItem(player);
        }

        public override float UseTimeMultiplier(Player player)
        {
            if (player.altFunctionUse == 2)
                return 3.6f;

            return 1f;
        }
        public override float UseAnimationMultiplier(Player player)
        {
            if (player.altFunctionUse == 2)
                return 3.6f;

            return 1f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                float SpeedX = velocity.X + Main.rand.Next(-10, 11) * 0.05f;
                float SpeedY = velocity.Y + Main.rand.Next(-10, 11) * 0.05f;
                Projectile.NewProjectile(source, position.X, position.Y - 4, SpeedX, SpeedY, ProjectileID.JackOLantern, (int)(damage * 1.4), knockback * 4f, player.whoAmI);
            }
            else
            {
                float SpeedX = velocity.X + Main.rand.Next(-10, 11) * 0.05f;
                float SpeedY = velocity.Y + Main.rand.Next(-10, 11) * 0.05f;
                Projectile.NewProjectile(source, position.X, position.Y - 4, SpeedX, SpeedY, type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(0, 100) < 33)
                return false;
            return true;
        }
        public override void UseItemFrame(Player player)
        {
            player.ChangeDir(Math.Sign((player.LocalMouseWorld() - player.Center).X));

            if (player.altFunctionUse == 2)
            {
                float animProgress = 0.5f - player.itemTime / (float)player.itemTimeMax;
                // ��������ת
                float rotation = (player.Center - player.LocalMouseWorld()).ToRotation() * player.gravDir + MathHelper.PiOver2;
                float offset = -0.05f * (float)Math.Pow((0.6f - animProgress) / 0.6f, 2);
                if (animProgress < 0.4f)
                    rotation += offset * player.direction;

                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rotation);
                CIFunction.NoHeldProjUpdateAim(player, MathHelper.ToDegrees(offset), 1);
            }
            else
            {
                CIFunction.NoHeldProjUpdateAim(player, 0, 1);
                float rotation = (player.Center - player.LocalMouseWorld()).ToRotation() * player.gravDir + MathHelper.PiOver2;
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rotation);
            }
        }
        public override void AddRecipes()
        {

            CreateRecipe().
                AddIngredient(ItemID.Pumpkin, 30).
                AddIngredient(ItemID.PumpkinSeed, 5).
                AddIngredient(ItemID.IllegalGunParts).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}