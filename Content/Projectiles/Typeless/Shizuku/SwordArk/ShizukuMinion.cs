using CalamityInheritance.Content.Items.Weapons.Typeless.ShizukuItem;
using CalamityInheritance.UI.MusicUI.MusicButton;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku.SwordArk
{
    public class ShizukuMinion : ModProjectile, ILocalizedModType
    {
        public override string Texture => GetInstance<ShizukuSword>().Texture;
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.noEnchantments = true;
            Projectile.noEnchantmentVisuals = true;
            Projectile.extraUpdates = 0;
        }
        public override void AI()
        {
            UpdateSwordPosition();
            UpdateAttack();
        }
        private void UpdateSwordPosition()
        {
        }

        private void UpdateAttack()
        {
        }
        SpriteBatch SB { get => Main.spriteBatch; }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.GetBaseDrawField(out Texture2D tex, out Vector2 drawPos, out Vector2 orig);
            SB.Draw(tex, drawPos, null, Color.White, Projectile.rotation + MathHelper.PiOver4, orig, Projectile.scale, 0, 0);
            return base.PreDraw(ref lightColor);
        }
    }
}
