using CalamityInheritance.Content.Projectiles.Ranged.Guns;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.Rarities;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Guns
{
    public class ClaretCannonLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Ranged;
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 30;
            Item.damage = 140;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 19;
            Item.useAnimation = 19;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5.5f;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<AbsoluteGreen>();
            Item.UseSound = SoundID.Item40;
            Item.autoReuse = true;
            Item.shootSpeed = 24f;
            Item.shoot = ProjectileType<ClaretCannonProjLegacy>();
            Item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-5, 0);

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BloodstoneCore>(4).
                AddTile(TileID.LunarCraftingStation).
                Register();
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
            CIFunction.NoHeldProjUpdateAim(player, MathHelper.ToDegrees(offset), 1);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 3; i++)
            {
                Projectile.NewProjectile(source, position - Vector2.UnitY * 10, velocity.RotatedByRandom(0.02f) * Main.rand.NextFloat(0.8f, 1.1f), ProjectileType<ClaretCannonProjLegacy>(), damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
}
