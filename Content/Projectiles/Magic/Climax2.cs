using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class Climax2 : ModProjectile, ILocalizedModType   
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        private double timeElapsed = 0.0;
        private double circleSize = 1.0;
        private double circleGrowth = 0.02;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.light = 0.5f;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 420;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            timeElapsed += 0.02;
            Projectile.velocity.X = (float)(Math.Sin(timeElapsed * (double)(0.5f * Projectile.ai[0])) * circleSize);
            Projectile.velocity.Y = (float)(Math.Cos(timeElapsed * (double)(0.5f * Projectile.ai[0])) * circleSize);
            circleSize += circleGrowth;

            if (Projectile.velocity.X > 0f)
            {
                Projectile.rotation += (Math.Abs(Projectile.velocity.Y) + Math.Abs(Projectile.velocity.X)) * 0.001f;
            }
            else
            {
                Projectile.rotation -= (Math.Abs(Projectile.velocity.Y) + Math.Abs(Projectile.velocity.X)) * 0.001f;
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame > 4)
                {
                    Projectile.frame = 0;
                }
            }

            CalamityUtils.MagnetSphereHitscan(Projectile, 300f, 8f, 4f, 5, ProjectileType<ClimaxBeam2>());
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft < 30)
            {
                float num7 = Projectile.timeLeft / 30f;
                Projectile.alpha = (int)((255f - 255f * num7) *0.5f);
            }
            return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 0);
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture2D13 = TextureAssets.Projectile[Projectile.type].Value;
            int num214 = TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type];
            int y6 = num214 * Projectile.frame;
            Main.spriteBatch.Draw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, y6, texture2D13.Width, num214)), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(texture2D13.Width / 2f, num214 / 2f), Projectile.scale, SpriteEffects.None, 0f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor.R = (byte)(lightColor.R * 0.5f);
            lightColor.G = (byte)(lightColor.G * 0.5f);

            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor * 0.5f, 1);
            return false;
        }
    }
}
