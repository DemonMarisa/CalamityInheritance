using System;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Accessories;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class MagnusProj: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 16;
            Projectile.friendly = true;
            Projectile.extraUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10; 
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.alpha = 0;
        }
        public override void AI()
        {
            //直接追踪
            CIFunction.HomeInOnNPC(Projectile, false, 1800f, 24f, 20f);
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;
            //绘制粒子
            DrawDust();
        }
        public override void OnKill(int timeLeft)
        {
            int dustType1 = 263;
            int dustType2 = 263;
            int height = 50;
            float scale1 = 1.7f;
            float scale2 = 0.8f;
            float scale3 = 2f;
            Vector2 value3 = (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2();
            Vector2 velocity = value3 * Projectile.velocity.Length() * (float)Projectile.MaxUpdates;
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            Projectile.ExpandHitboxBy(height);
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Damage();
            for (int i = 0; i < 40; i++)
            {
                int dustType = Utils.SelectRandom(Main.rand,
                [
                    56,
                    92,
                    229,
                    206,
                    181
                ]);
                int idx = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 200, default, scale1);
                Dust dust = Main.dust[idx];
                dust.position = Projectile.Center + Vector2.UnitY.RotatedByRandom(Math.PI) * (float)Main.rand.NextDouble() * (float)Projectile.width / 2f;
                dust.noGravity = true;
                dust.velocity *= 3f;
                dust.velocity += velocity * Main.rand.NextFloat();
                idx = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType1, 0f, 0f, 100, default, scale2);
                dust.position = Projectile.Center + Vector2.UnitY.RotatedByRandom(Math.PI) * (float)Main.rand.NextDouble() * (float)Projectile.width / 2f;
                dust.velocity *= 2f;
                dust.noGravity = true;
                dust.fadeIn = 1f;
                dust.color = Color.Crimson * 0.5f;
                dust.velocity += velocity * Main.rand.NextFloat();
            }
            for (int i = 0; i < 20; i++)
            {
                int idx = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType2, 0f, 0f, 0, default, scale3);
                Dust dust = Main.dust[idx];
                dust.position = Projectile.Center + Vector2.UnitX.RotatedByRandom(Math.PI).RotatedBy((double)Projectile.velocity.ToRotation(), default) * (float)Projectile.width / 3f;
                dust.noGravity = true;
                dust.velocity *= 0.5f;
                dust.velocity += velocity * (0.6f + 0.6f * Main.rand.NextFloat());
            }
        }

        public void DrawDust()
        {
            int dTypeRand = Utils.SelectRandom(Main.rand,
            [
                56,
                92,
                229,
                206,
                181
            ]);
            int dType = 261;
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] == 48f)
                Projectile.localAI[0] = 0f;
            else if (Projectile.alpha == 0)
            {
                for (int i = 0; i < 2; i++)
                     MainDust(dType, i);
            }

            //更多粒子...
            if (Main.rand.NextBool(12))
                MoreDust(DustID.BoneTorch, Projectile.width, Projectile.height,0.2, 100, 1f, 0.1f, Projectile.velocity*2f, false, 0.9f);

            if (Main.rand.NextBool(64))
                MoreDust(DustID.BoneTorch, Projectile.width, Projectile.height,0.4, 155, 1f, 0.3f, Vector2.Zero, true, 1.4f);

            if (Main.rand.NextBool(4))
            {
                for (int j = 0; j < 2; j++)
                MoreDust(dTypeRand, Projectile.width, Projectile.height,0.8, 0, 1.2f, 0.3f, Vector2.Zero, true, 1.4f, true);
            }

            if (Main.rand.NextBool(3))
                MoreDust(dType, Projectile.width, Projectile.height,0.2, 100, 1f, 0.3f, Vector2.Zero, false, 1.2f, true);
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.25f / 255f, (255 - Projectile.alpha) * 0f / 255f, (255 - Projectile.alpha) * 0.25f / 255f);
            int sizeBoost = 14;
            int newSizeW = Projectile.width - sizeBoost * 2;
            int newSizeH = Projectile.height - sizeBoost * 2;
            for (int i = 0; i < 2; i++)
            {
                EvenMoreDust(newSizeW, newSizeH, DustID.PortalBolt, 100, 1.35f, 0.1f, Projectile.velocity * 0.5f, true);
            }
            if (Main.rand.NextBool(8))
            {
                sizeBoost = 16;
                newSizeW = Projectile.width - sizeBoost * 2;
                newSizeH = Projectile.height - sizeBoost * 2;
                EvenMoreDust(newSizeW, newSizeH, DustID.PortalBolt, 100, 1.35f, 0.25f, Projectile.velocity * 0.5f, true);
            }

        }
        /// <summary>
        /// 进一步强行封装节约空间
        /// </summary>
        /// <param name="dWidth">粒子宽度</param>
        /// <param name="dHeight">粒子高度</param>
        /// <param name="dType">粒子类型</param>
        /// <param name="dAlpha">粒子透明度</param>
        /// <param name="dScale">粒子大小</param>
        /// <param name="dVel">粒子速度</param>
        /// <param name="dVelEx">额外粒子速度</param>
        /// <param name="ifNoGrav">是否无视重力</param>
        public void EvenMoreDust(int dWidth, int dHeight, int dType, int dAlpha, float dScale, float dVel, Vector2 dVelEx, bool ifNoGrav)
        {
            int d = Dust.NewDust(Projectile.position, dWidth, dHeight, dType, 0, 0, dAlpha, default, dScale);
            Main.dust[d].noGravity = ifNoGrav;
            Main.dust[d].velocity *= dVel;
            Main.dust[d].velocity += dVelEx;
        }
        /// <summary>
        /// 尽可能简化AI的内容的强行封装，反正就是为了生成飞行粒子做的
        /// </summary>
        /// <param name="dType">粒子类型</param>
        /// <param name="rotatedValue">随机旋转的值，我也不太明白，但反正是一个低于1的double数据</param>
        /// <param name="dWidth">粒子宽度</param>
        /// <param name="dHeight">粒子高度</param>
        /// <param name="alpha">粒子透明度</param>
        /// <param name="scale">粒子大小</param>
        /// <param name="velValue">粒子的速度值</param>
        /// <param name="sinOffset">粒子位置的偏移</param>
        /// <param name="needFadeBool">是否需要让淡入淡出效果变成随机1/2概率</param>
        /// <param name="fadeValue">淡出的值，默认取1f</param>
        /// <param name="noGrav">是否无视重力，默认取否</param>
        public void MoreDust(int dType, int dWidth, int dHeight,double rotatedValue, int alpha, float scale, float velValue, Vector2 sinOffset, bool needFadeBool, float? fadeValue = 1f, bool? noGrav = false)
        {
            bool dontGrav = noGrav ?? false;
            float fadeValueReal = fadeValue ?? 1f;
            Vector2 offset = -Vector2.UnitX.RotatedByRandom(rotatedValue).RotatedBy((double)Projectile.velocity.ToRotation(), default);
            int d = Dust.NewDust(Projectile.position, dWidth, dHeight, dType, 0f, 0f, alpha, default, scale);
            Main.dust[d].noGravity = dontGrav;
            Main.dust[d].velocity *= velValue;
            Main.dust[d].position = Projectile.Center + offset * Projectile.width / 2f + sinOffset;
            if (!needFadeBool)Main.dust[d].fadeIn = fadeValueReal;
            if (needFadeBool) 
            if (Main.rand.NextBool())
                Main.dust[d].fadeIn = fadeValueReal;
        }
        public void MainDust(int dType, int i)
        {
            Vector2 offset = Vector2.UnitX * -30f;
            offset = -Vector2.UnitX.RotatedBy((double)(Projectile.localAI[0] * 0.13f + i * MathHelper.Pi), default) *
                     new Vector2(10f, 20f) - Projectile.rotation.ToRotationVector2() * 10f;

            int d = Dust.NewDust(Projectile.Center, 0,0, dType, 0f, 0f, 160, default, 1f);
            Main.dust[d].noGravity = true;
            Main.dust[d].position = Projectile.Center + offset + Projectile.velocity * 2f;
            Main.dust[d].velocity = Vector2.Normalize(Projectile.Center + Projectile.velocity * 2f * 8f - Main.dust[d].position) * 2f + Projectile.velocity * 2f;
        }
    }
}