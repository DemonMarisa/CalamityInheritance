using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.ExoWeapons;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Rarity.Special;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.ExoWeapons
{
    public class ExoFlail : CIExoWeapon
    {

        public static float Speed = 34f;

        public static float MouseHomingAcceleration = 0.75f;

        public static float MaxRange = 780f;

        public static float ReturnSpeed = 40f;

        private int hitCount = 0;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 90;
            Item.DamageType = TrueMelee.Instance;
            Item.damage = 1000;
            Item.knockBack = 9f;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item101;
            Item.channel = true;
            Item.rare = RarityType<SeraphPurple>();
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.shoot = ProjectileType<ExoFlailProj>();
            Item.shootSpeed = 24f;
        }
        public override bool MeleePrefix() => true;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (UseAlt)
            {
                hitCount++;
                float ai3 = (Main.rand.NextFloat() - 0.75f) * 0.7853982f;
                if (hitCount >= 5)
                {
                    Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileType<ExoFlailProj2>(), damage, knockback, player.whoAmI, 0f, 0f, 1f);
                    hitCount = 0;
                }
                else
                {
                    Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI, 0f, ai3);
                }
            }
            else
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileType<ExoFlailProj2>(), damage, knockback, player.whoAmI, 0f);
            }
            return false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
        }
        public override void AddRecipes()
        {
        }
    }
}
