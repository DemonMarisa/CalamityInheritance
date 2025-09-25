using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.CalProjChange
{
    public class PhotovisceratorLaser : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 4400;
        }
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 6;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
            Projectile.timeLeft = 16;
            Projectile.Opacity = 0.5f;
            Projectile.localNPCHitCooldown = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.tileCollide = false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // Otherwise, perform an AABB line collision check to check the whole beam.
            float _ = float.NaN;
            bool c = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * 10000, 24f, ref _);
            return c;
        }
        public override void OnSpawn(IEntitySource source)
        {
            // 初始化5个随机旋转值
            for (int i = 0; i < 5; i++)
            {
                Projectile.CalamityInheritance().ProjNewAI[i] = Main.rand.NextFloat(MathHelper.TwoPi);
            }
        }

        public override void AI()
        {
            HandleOpacity();
            UpdateLocalAI();
            UpdatePositionAndDirection();
            AddLightEffects();
        }
        #region 光束更亮
        public void HandleOpacity()
        {
            // 透明度渐变处理
            Projectile.Opacity = Math.Min(Projectile.Opacity * 2f, 1f);
        }
        #endregion
        #region 更新绘制相关的ai
        public void UpdateLocalAI()
        {
            const float sustainedBeamSpeed = 5f;
            const float instantBeamSpeed = 2f;
            // 是否为瞬发光束，1为持续发射使用的，淡入淡出较快，0为瞬间发射使用的，淡入淡出较慢
            bool isSustainedBeam = Projectile.ai[2] == 1f;
            // 瞬间的光束会增加的更慢
            Projectile.localAI[0] += isSustainedBeam ? sustainedBeamSpeed : instantBeamSpeed;
            // 瞬间的光束会消散的更快
            if (Projectile.timeLeft <= (isSustainedBeam ? 4f : 8f))
                Projectile.localAI[0] -= isSustainedBeam ? 10f : 3f;
        }
        #endregion
        #region 更新朝向
        public void UpdatePositionAndDirection()
        {
            // 是否是持续的光束
            bool isSustainedBeam = Projectile.ai[2] == 1f;
            int parent = (int)Projectile.ai[1];
            if (isSustainedBeam)
            {
                // 更新位置
                Vector2 mountedCenter = Main.projectile[parent].Center;
                Projectile.Center = mountedCenter + Projectile.velocity.SafeNormalize(Vector2.UnitY);
                // 更新方向
                Vector2 Projdirection = Vector2.UnitX.RotatedBy(Main.projectile[parent].rotation);
                Projdirection.SafeNormalize(Vector2.UnitX);
                Projectile.velocity = Projdirection;
            }
        }
        #endregion
        #region 让光束发光
        public void AddLightEffects()
        {
            const int lightSegments = 139;
            const float lightSpacing = 32f;
            float fadeMultiplier = Math.Max(1, (Projectile.timeLeft - 6) / 6f);

            for (int i = 0; i < lightSegments; i++)
            {
                Vector2 lightPosition = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.UnitY) * i * lightSpacing;
                Lighting.AddLight(lightPosition, 0.2f * fadeMultiplier, 0.4f * fadeMultiplier, 0.8f * fadeMultiplier);
            }
        }
        #endregion
        #region 绘制
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        #endregion
    }
}
