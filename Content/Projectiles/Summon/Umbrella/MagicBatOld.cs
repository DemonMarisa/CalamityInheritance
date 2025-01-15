using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;

namespace CalamityInheritance.Content.Projectiles.Summon.Umbrella
{
    public class MagicBatOld : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Summon";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Baseball Bat");
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 150;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }
        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 0.785f;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(80, 200, 120, Projectile.alpha);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 dspeed = new Vector2(Main.rand.NextFloat(-7f, 7f), Main.rand.NextFloat(-7f, 7f));
                int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.IceRod, dspeed.X, dspeed.Y, 50, default, 1.2f);
                Main.dust[dust].noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
