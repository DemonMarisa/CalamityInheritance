using CalamityInheritance.Assets;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.Utils
{
    public static partial class CIUtils
    {
        /// <summary>
        /// 计算受重力影响下的发射速度，使弹幕能够命中目标。
        /// </summary>
        /// <param name="origin">发射点</param>
        /// <param name="target">目标点</param>
        /// <param name="gravity">重力加速度（正值，向下）</param>
        /// <param name="speed">期望的发射速度大小（必须大于0）</param>
        /// <returns>修正后的速度向量，大小等于 speed，方向已抬高以补偿重力</returns>
        public static Vector2 GetGravityCompensatedVelocity(Vector2 origin, Vector2 target, float gravity, float speed)
        {
            Vector2 diff = target - origin;
            float dx = diff.X;

            // 水平距离过小时直接发射（避免除零）
            float horizontalDist = Math.Abs(dx);
            if (horizontalDist < 0.001f)
            {
                // 正上/正下，重力修正没有意义，直接朝向目标
                return diff.SafeNormalize(Vector2.UnitY) * speed;
            }

            // 估算飞行时间（假设水平速度 ≈ 总速度，因为抬升角度通常不大）
            float t = horizontalDist / speed;
            if (t < 0.01f) t = 0.01f;

            // 重力造成的下落量： drop = 0.5 * g * t²
            float drop = 0.5f * gravity * t * t;

            // 抬高目标点（Y轴向下，所以减去drop）
            Vector2 compensatedTarget = new Vector2(target.X, target.Y - drop);

            // 计算修正后的方向并乘以速度大小
            Vector2 direction = compensatedTarget - origin;
            if (direction.LengthSquared() < 0.0001f)
                direction = Vector2.UnitY;

            return direction.SafeNormalize(Vector2.Zero) * speed;
        }
        public static Vector2 GetProjectilePhysicsFiringVelocity(Vector2 shootingPosition, Vector2 destination, float gravity, float shootSpeed, Vector2? nanFallback = null)
        {
            // Ensure that the gravity has the right sign for Terraria's coordinate system.
            gravity = -Math.Abs(gravity);

            float horizontalRange = MathHelper.Distance(shootingPosition.X, destination.X);
            float fireAngleSine = gravity * horizontalRange / (float)Math.Pow(shootSpeed, 2);

            // Clamp the sine if no fallback is provided.
            if (nanFallback is null)
                fireAngleSine = MathHelper.Clamp(fireAngleSine, -1f, 1f);

            float fireAngle = (float)Math.Asin(fireAngleSine) * 0.5f;

            // Get out of here if no valid firing angle exists. This can only happen if a fallback does indeed exist.
            if (float.IsNaN(fireAngle))
                return nanFallback.Value * shootSpeed;

            Vector2 fireVelocity = new Vector2(0f, -shootSpeed).RotatedBy(fireAngle);
            fireVelocity.X *= (destination.X - shootingPosition.X < 0).ToDirectionInt();
            return fireVelocity;
        }

        public static void KillShootProjectiles(bool shouldBreak, int projType, Player player)
        {
            for (int x = 0; x < Main.maxProjectiles; x++)
            {
                Projectile proj = Main.projectile[x];
                if (proj.active && proj.owner == player.whoAmI && proj.type == projType)
                {
                    proj.Kill();
                    if (shouldBreak)
                        break;
                }
            }
        }
        public static int CountProjectiles(int projectileID)
        {
            int count = 0;
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.type == projectileID && proj.active)
                    count++;
            }
            return count;
        }

        /// <summary>
        /// Detects nearby hostile NPCs from a given point with minion support
        /// </summary>
        /// <param name="origin">The position where we wish to check for nearby NPCs</param>
        /// <param name="maxDistanceToCheck">Maximum amount of pixels to check around the origin</param>
        /// <param name="owner">Owner of the minion</param>
        /// <param name="ignoreTiles">Whether to ignore tiles when finding a target or not</param>
        public static NPC MinionHoming(this Vector2 origin, float maxDistanceToCheck, Player owner, bool ignoreTiles = true, bool checksRange = false)
        {
            if (owner is null || !owner.whoAmI.InBounds(Main.maxPlayers) || !owner.MinionAttackTargetNPC.InBounds(Main.maxNPCs))
                return LAPUtilities.FindClosestTarget(origin, maxDistanceToCheck, ignoreTiles);
            NPC npc = Main.npc[owner.MinionAttackTargetNPC];
            bool canHit = true;
            if (!ignoreTiles)
                canHit = Collision.CanHit(origin, 1, 1, npc.Center, 1, 1);
            float extraDistance = (npc.width / 2) + (npc.height / 2);
            bool distCheck = Vector2.Distance(origin, npc.Center) < (maxDistanceToCheck + extraDistance) || !checksRange;
            if (owner.HasMinionAttackTargetNPC && canHit && distCheck)
            {
                return npc;
            }
            return LAPUtilities.FindClosestTarget(origin, maxDistanceToCheck, ignoreTiles);
        }
        public static T ModProjectile<T>(this Projectile projectile) where T : ModProjectile
        {
            return projectile.ModProjectile as T;
        }
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
        public static void BouncingOnTiles(this Projectile proj, Vector2 oldVel)
        {
            if (proj.velocity.X != oldVel.X)
                proj.velocity.X = -oldVel.X;
            if (proj.velocity.Y != -oldVel.Y)
                proj.velocity.Y = oldVel.Y;
        }
        public static void BouncingOnTiles(this Projectile proj, Vector2 oldVel, Vector2 wantedNewVel)
        {
            if (proj.velocity.X != oldVel.X)
                proj.velocity.X = wantedNewVel.X;
            if (proj.velocity.Y != -oldVel.Y)
                proj.velocity.Y = wantedNewVel.Y;
        }
        public static void BouncingOnTiles(this Projectile proj, Vector2 oldVel, float newVelX, float newVelY)
        {
            if (proj.velocity.X != oldVel.X)
                proj.velocity.X = newVelX;
            if (proj.velocity.Y != -oldVel.Y)
                proj.velocity.Y = newVelX;
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
        /// <summary>
        /// Creates an explosion which is visually identical to vanilla's Rocket III and Rocket IV on-hit explosions.
        /// </summary>
        /// <param name="projectile">The projectile which is exploding.</param>
        public static void LargeFieryExplosion(this Projectile projectile)
        {
            // Sparks and such
            Vector2 corner = projectile.position;
            for (int i = 0; i < 40; i++)
            {
                int idx = Dust.NewDust(corner, projectile.width, projectile.height, DustID.Smoke, 0f, 0f, 100, default, 2f);
                Main.dust[idx].velocity *= 3f;
                if (Main.rand.NextBool())
                {
                    Main.dust[idx].scale = 0.5f;
                    Main.dust[idx].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int i = 0; i < 70; i++)
            {
                int idx = Dust.NewDust(corner, projectile.width, projectile.height, DustID.Torch, 0f, 0f, 100, default, 3f);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity *= 5f;
                idx = Dust.NewDust(corner, projectile.width, projectile.height, DustID.Torch, 0f, 0f, 100, default, 2f);
                Main.dust[idx].velocity *= 2f;
            }

            // Smoke, which counts as a Gore
            if (!Main.dedServ)
            {
                Vector2 goreSource = projectile.Center;
                int goreAmt = 3;
                Vector2 source = new Vector2(goreSource.X - 24f, goreSource.Y - 24f);
                for (int goreIndex = 0; goreIndex < goreAmt; goreIndex++)
                {
                    float velocityMult = 0.33f;
                    if (goreIndex < (goreAmt / 3))
                    {
                        velocityMult = 0.66f;
                    }
                    if (goreIndex >= (2 * goreAmt / 3))
                    {
                        velocityMult = 1f;
                    }
                    int type = Main.rand.Next(61, 64);
                    int smoke = Gore.NewGore(projectile.GetSource_Death(), source, default, type, 1f);
                    Gore gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X += 1f;
                    gore.velocity.Y += 1f;
                    type = Main.rand.Next(61, 64);
                    smoke = Gore.NewGore(projectile.GetSource_Death(), source, default, type, 1f);
                    gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X -= 1f;
                    gore.velocity.Y += 1f;
                    type = Main.rand.Next(61, 64);
                    smoke = Gore.NewGore(projectile.GetSource_Death(), source, default, type, 1f);
                    gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X += 1f;
                    gore.velocity.Y -= 1f;
                    type = Main.rand.Next(61, 64);
                    smoke = Gore.NewGore(projectile.GetSource_Death(), source, default, type, 1f);
                    gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X -= 1f;
                    gore.velocity.Y -= 1f;
                }
            }
        }
        #region 绘制方法
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
        public static bool DrawBeam(this Projectile projectile, float length, float spacer, Color lightColor, Texture2D texture = null, bool curve = false)
        {
            if (texture is null)
                texture = TextureAssets.Projectile[projectile.type].Value;

            float widthOffset = (float)(texture.Width - projectile.width) * 0.5f + (float)projectile.width * 0.5f;
            float heightOffset = (float)(projectile.height / 2);
            Vector2 origin = new Vector2(widthOffset, heightOffset);
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (projectile.spriteDirection == -1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Rectangle roughScreenBounds = new Rectangle((int)Main.screenPosition.X - 500, (int)Main.screenPosition.Y - 500, Main.screenWidth + 1000, Main.screenHeight + 1000);
            if (projectile.getRect().Intersects(roughScreenBounds))
            {
                Vector2 drawPos = projectile.position - Main.screenPosition + origin;
                drawPos.Y += projectile.gfxOffY;
                float maxTrailPoints = length;

                if (projectile.ai[1] == 1f)
                    maxTrailPoints = (int)projectile.localAI[0];

                Vector2 cumulativeOffset = Vector2.Zero;
                Color alpha = projectile.GetAlpha(lightColor);
                float fixedRotation = projectile.rotation + MathHelper.PiOver2;
                for (int i = 1; i <= (int)projectile.localAI[0]; i++)
                {
                    Vector2 velToUseThisIter = projectile.velocity;
                    if (curve)
                    {
                        float oldVelRatio = i / projectile.localAI[0];
                        int oldVelIndex = (int)(oldVelRatio * projectile.oldRot.Length);
                        if (oldVelIndex > 0)
                        {
                            float angleChange = projectile.oldRot[oldVelIndex - 1] - projectile.rotation;
                            velToUseThisIter = projectile.velocity.RotatedBy(angleChange);
                        }
                    }
                    cumulativeOffset += Vector2.Normalize(velToUseThisIter) * spacer;
                    Color color = alpha;
                    color *= (maxTrailPoints - (float)i) / maxTrailPoints;
                    color.A = 0;
                    Main.spriteBatch.Draw(texture, drawPos - cumulativeOffset, null, color, fixedRotation, origin, projectile.scale, spriteEffects, 0f);
                }
            }
            return false;
        }
        #endregion
    }
}
