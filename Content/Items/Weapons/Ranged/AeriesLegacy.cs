using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class AeriesLegacy : CIRanged, ILocalizedModType
    {
        public override void SetDefaults()
        {
            Item.damage = 35;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 56;
            Item.height = 30;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5.5f;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item41;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;
            Item.shoot = ProjectileType<AeriesShockblastRound>();
            Item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, -3);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 offset = new(0, -10);
            if(CIConfig.Instance.AmmoConversion)
            {
                Projectile.NewProjectile(source, position + offset, velocity, ProjectileType<AeriesShockblastRound>(), damage, knockback, player.whoAmI);
            }
            else
            {
                if (type == ProjectileID.Bullet)
                    Projectile.NewProjectile(source, position + offset, velocity, ProjectileType<AeriesShockblastRound>(), damage, knockback, player.whoAmI);
                else
                    Projectile.NewProjectile(source, position + offset, velocity, type, damage, knockback, player.whoAmI);
            }
            return false;
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

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<CursedCapper>())
                .AddIngredient(ItemID.FallenStar, 3)
                .AddIngredient(ItemID.ShroomiteBar, 5)
                .AddIngredient(ItemType<EssenceofSunlight>())
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
