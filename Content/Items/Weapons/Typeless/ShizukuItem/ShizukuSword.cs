using System.Collections.Generic;
using CalamityInheritance.Content.Projectiles.Typeless.Shizuku;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.Sounds.Custom;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Typeless.ShizukuItem
{
    public class ShizukuSword : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Typeless";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.damage = 114;
            Item.width = 100;
            Item.height = 112;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = CIShopValue.RarityPricePureRed;
            Item.rare = ModContent.RarityType<ShizukuAqua>();
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ShizukuSwordProjectile>();
            // Item.UseSound = SoundID.Item82;
            Item.shootSpeed = 12f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            #region 
            /*
            int count = 2;
            int proj = ModContent.ProjectileType<ShizukuSwordProjectile>();
            for (int i = -1; i < count; i += 2)
            {
                Vector2 setSpeed = velocity.RotatedBy(MathHelper.PiOver4 / 1.5f * i) * 2f;
                int p = Projectile.NewProjectile(source, position, -(velocity + setSpeed), proj, damage, knockback, player.whoAmI);
                if (Main.rand.Next(-1, 2) == i)
                {
                    Main.projectile[p].scale *= 0.85f;
                    Main.projectile[p].velocity.RotatedBy(MathHelper.PiOver4 / 8f);
                }
                
            }
            int proj2 = ModContent.ProjectileType<MoonPlaceholder>();
            Vector2 newspeed = new Vector2(30f, 0f).RotatedBy(-MathHelper.PiOver2);
            if (player.ownedProjectileCounts[proj2] < 1)
                Projectile.NewProjectile(source, position, newspeed, proj2, 0, 0f, player.whoAmI);
            */
            #endregion
            int proj = ModContent.ProjectileType<ShizukuShockwave>();
            Projectile.NewProjectile(source, Main.MouseWorld, velocity * 0.1f, proj, damage, knockback, player.whoAmI);
            return false;
        }
    }
}