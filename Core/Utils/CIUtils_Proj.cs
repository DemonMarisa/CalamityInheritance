using CalamityInheritance.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace CalamityInheritance.Core.Utils
{
    public static partial class CIUtils
    {      
        /// <summary>
        /// 播放射弹帧图
        /// </summary>
        /// <param name="projectile">射弹</param>
        /// <param name="fCounter">计时器，即间隔多少时间播放下一张帧图</param>
        /// <param name="fMax">这个帧图最大的帧数</param>
        public static int FramesChanger(this Projectile projectile, int fCounter, int fMax)
        {
            projectile.frameCounter++;
            if (projectile.frameCounter > fCounter)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
            if (projectile.frame >= fMax)
                projectile.frame = 0;
            return projectile.frame;
        }
        public static void DrawStarTrail(this Projectile projectile, Color outer, Color inner, float auraHeight = 10f)
        {
            Texture2D aura = CITextureRegister.StarTrail.Value;
            Vector2 offsets = new Vector2(0f, projectile.gfxOffY) - Main.screenPosition;
            Rectangle auraRec = aura.Frame();
            float auraRotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Vector2 auraOrigin = new Vector2(auraRec.Width / 2f, auraHeight);

            // Outer trail
            Vector2 drawStartOuter = offsets + projectile.Center + projectile.velocity;
            Vector2 spinPoint = -Vector2.UnitY * auraHeight;
            float time = Main.player[projectile.owner].miscCounter % 216000f / 60f;
            Color outerColor = outer * 0.2f;
            outerColor.A = 0;
            float rotation = MathHelper.TwoPi * time;
            for (int o = 0; o < 6; o += 2)
            {
                Vector2 spinStart = drawStartOuter + spinPoint.RotatedBy(rotation - MathHelper.Pi * o / 3f);
                float scaleMultOuter = 1.5f - o * 0.1f;
                Main.EntitySpriteDraw(aura, spinStart, auraRec, outerColor, auraRotation, auraOrigin, scaleMultOuter, SpriteEffects.None, 0);
            }

            // Inner trail
            Vector2 drawStartInner = offsets + projectile.Center - projectile.velocity * 0.5f;
            Color innerColor = inner * 0.5f;
            innerColor.A = 0;
            for (float i = 0f; i < 1f; i += 0.5f)
            {
                float scaleMult = time % 0.5f / 0.5f;
                scaleMult = (scaleMult + i) % 1f;
                float colorMult = scaleMult * 2f;
                if (colorMult > 1f)
                    colorMult = 2f - colorMult;

                Main.EntitySpriteDraw(aura, drawStartInner, auraRec, innerColor * colorMult, auraRotation, auraOrigin, 0.3f + scaleMult * 0.5f, SpriteEffects.None, 0);
            }
        }
    }
}
