using CalamityInheritance.Assets;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Core.Utils
{
    public static partial class CIUtils
    {
        /// <summary>
        /// 直接设置<see cref="ProjectileID.Sets.TrailCacheLength"/>和<see cref="ProjectileID.Sets.TrailingMode"/>的拓展方法
        /// <br><paramref name="length"/>默认取4，而<paramref name="mode"/>默认值为2</br>
        /// </summary>
        public static void SetTrail(this Projectile proj, int length = 4, int mode = 2)
        {
            ProjectileID.Sets.TrailCacheLength[proj.type] = length;
            ProjectileID.Sets.TrailingMode[proj.type] = mode;
        }
        /// <summary>
        /// ……我不理解这个方法是图什么。
        /// <br>灾厄写这个好像是用来边界检查的……？但是这有必要吗</br>
        /// <br>反正主要是搬运的时候顺带复制过来的，你看需求吧</br>
        /// </summary>
        /// <param name="index"></param>
        /// <param name="cap"></param>
        /// <returns></returns>
        public static bool InBounds(this int index, int cap)
        {
            if (index >= 0)
            {
                return index < cap;
            }

            return false;
        }
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
        public static Projectile NewProj(this Projectile projectile, int type, Vector2 position, Vector2 vel, float damageMult = 1f, float knockbackmult = 1f, float ai0 = 0, float ai1 = 0, float ai2 = 0)
        {
            if (projectile.IsLocalPlayer())
                return Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), position, vel, type, (int)(projectile.damage * damageMult), projectile.knockBack * knockbackmult, projectile.owner, ai0, ai1, ai2);
            else
                return null;
        }
        public static Projectile ProjectileBarrage(IEntitySource source, Vector2 originVec, Vector2 targetPos, bool fromRight, float xOffsetMin, float xOffsetMax, float yOffsetMin, float yOffsetMax, float projSpeed, int projType, int damage, float knockback, int owner, bool clamped = false, float inaccuracyOffset = 5f)
        {
            float xPos = originVec.X + Main.rand.NextFloat(xOffsetMin, xOffsetMax) * fromRight.ToDirectionInt();
            float yPos = originVec.Y + Main.rand.NextFloat(yOffsetMin, yOffsetMax) * Main.rand.NextBool().ToDirectionInt();
            Vector2 spawnPosition = new Vector2(xPos, yPos);
            Vector2 velocity = targetPos - spawnPosition;
            velocity.X += Main.rand.NextFloat(-inaccuracyOffset, inaccuracyOffset);
            velocity.Y += Main.rand.NextFloat(-inaccuracyOffset, inaccuracyOffset);
            velocity.Normalize();
            velocity *= projSpeed * (clamped ? 150f : 1f);
            //This clamp means the spawned projectiles only go at diagnals and are not accurate
            if (clamped)
            {
                velocity.X = MathHelper.Clamp(velocity.X, -15f, 15f);
                velocity.Y = MathHelper.Clamp(velocity.Y, -15f, 15f);
            }
            return Projectile.NewProjectileDirect(source, spawnPosition, velocity, projType, damage, knockback, owner);
        }
        public static Projectile FireToClostNPC(this Projectile proj, int type, Vector2 firePos, float Speed, float distance, float damageMult = 1f, float knockBackMult = 1f, float ai0 = 0, float ai1 = 0, float ai2 = 0)
        {
            NPC npc = LAPUtilities.FindClosestTarget(proj.Center, distance);
            if (npc is not null)
            {
                Vector2 fireVel = LAPUtilities.GetVector2(proj.Center, npc.Center);
                return proj.NewProj(type, firePos, fireVel * Speed, damageMult, knockBackMult, ai0, ai1, ai2);
            }
            else
                return null;
        }
        public static Projectile ProjectileRain(IEntitySource source, Vector2 targetPos, float xLimit, float xVariance, float yLimitLower, float yLimitUpper, float projSpeed, int projType, int damage, float knockback, int owner)
        {
            float x = targetPos.X + Main.rand.NextFloat(-xLimit, xLimit);
            float y = targetPos.Y - Main.rand.NextFloat(yLimitLower, yLimitUpper);
            Vector2 spawnPosition = new Vector2(x, y);
            Vector2 velocity = targetPos - spawnPosition;
            velocity.X += Main.rand.NextFloat(-xVariance, xVariance);
            float targetDist = velocity.Length();
            targetDist = projSpeed / targetDist;
            velocity.X *= targetDist;
            velocity.Y *= targetDist;
            return Projectile.NewProjectileDirect(source, spawnPosition, velocity, projType, damage, knockback, owner);
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
