using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Items.Weapons.Magic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Items;

namespace CalamityInheritance.Content.Projectiles.Magic
{
    // ai0用于外部的光线缩放控制
    // ai1用于绑定父级
    // ai2用于判断是否为持续光束
    public class AlphaBeam : ModProjectile
    {
        public override LocalizedText DisplayName => CalamityUtils.GetItemName<AlphaRayLegacy>();
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
            Projectile.DamageType = DamageClass.Magic;
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
            if(isSustainedBeam)
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
            DrawLaserBeam();
            return false;
        }
        #region 绘制激光束
        public void DrawLaserBeam()
        {
            // 基础参数
            const int laserLength = 4400;
            float alphaMultiplier = Math.Max(0, Projectile.Opacity * Math.Min(1, Projectile.timeLeft / 3f));
            float beamRotation = Projectile.velocity.ToRotation();
            float Scale = Projectile.ai[2] == 1 ? 0.5f : 1.5f * Projectile.ai[0];
            // 颜色
            Color baseColor = Color.White * alphaMultiplier;
            Color Auxiliarycolor = Color.DodgerBlue * alphaMultiplier;
            baseColor.A = Auxiliarycolor.A = 0;

            // 纹理
            Texture2D mainTexture = TextureAssets.Projectile[Type].Value;
            Texture2D bloomTexture = Main.Assets.Request<Texture2D>("Images/Extra_197").Value;
            Texture2D headTexture = Main.Assets.Request<Texture2D>("Images/Projectile_927").Value;
            Texture2D tailTexture = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Healing/EssenceFlame").Value;

            DrawGlowEffects(headTexture, Auxiliarycolor, Scale);
            DrawBloomEffect(bloomTexture, Auxiliarycolor, beamRotation, laserLength, Scale);

            if (Projectile.timeLeft > 5f)
            {
                DrawMainBeam(mainTexture, baseColor, beamRotation, laserLength, Scale);
                DrawTailEffect(tailTexture, baseColor, beamRotation, laserLength, Scale);
            }
        }
        #endregion
        #region 绘制头部星星
        public void DrawGlowEffects(Texture2D headTexture, Color color, float Scale)
        {
            const int glowCount = 5;
            var projAI = Projectile.CalamityInheritance().ProjNewAI;

            for (int i = 0; i < glowCount; i++)
            {
                Rectangle rect = new Rectangle(0, 0, headTexture.Width / 2, headTexture.Height);
                Vector2 origin = new Vector2(headTexture.Width / 2, headTexture.Height / 2);
                Vector2 scale = new Vector2(
                    Projectile.localAI[0] / 20f / (Projectile.ai[2] + 1),
                    Projectile.localAI[0] / 20f / (Projectile.ai[2] + 1));

                Main.EntitySpriteDraw(headTexture, Projectile.Center - Main.screenPosition,
                    rect, color * 0.8f, projAI[i],// projAI[i]为绘制星星时的随机旋转
                    origin, scale * Scale,  
                    SpriteEffects.None);
            }
        }
        #endregion
        #region 绘制本体辉光
        public void DrawBloomEffect(Texture2D texture, Color color, float rotation, int length, float Scale)
        {
            Rectangle rect = new Rectangle(0, 0, length, texture.Height);
            Vector2 scale = new Vector2(1, Projectile.localAI[0] / 60f / (Projectile.ai[2] + 2));
            Vector2 origin = new Vector2(0, texture.Height / 2);
            Main.EntitySpriteDraw(texture, position: Projectile.Center - Main.screenPosition, rect,
                color, rotation, origin, scale * Scale,
                SpriteEffects.None);
        }
        #endregion
        #region 绘制主光束
        private void DrawMainBeam(Texture2D texture, Color color, float rotation, int length, float Scale)
        {
            Rectangle rect = new Rectangle(0, 0, length, texture.Height);
            Vector2 scale = new Vector2(1, Projectile.localAI[0] / 9f);
            Vector2 origin = new Vector2(0, texture.Height / 2);
            Main.EntitySpriteDraw(texture,Projectile.Center - Main.screenPosition,
                new Rectangle(0, 0, length, texture.Height),
                color, rotation, origin, scale * Scale,
                SpriteEffects.None);
        }
        // 你画的啥
        private void DrawTailEffect(Texture2D texture, Color color, float rotation, int length, float Scale)
        {
            Vector2 tailPosition = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * (length - 18) - Main.screenPosition;
            Vector2 origin = new Vector2(texture.Width, texture.Height / 4);
            Vector2 scale = new Vector2(0.5f, 0.5f);

            Rectangle rect = new Rectangle(0, 0, length, texture.Height / 4);
            Main.EntitySpriteDraw(texture, tailPosition, rect, color,
                rotation - MathHelper.PiOver2, origin, scale * Scale,
                SpriteEffects.None);
        }
        #endregion
        #endregion
    }
}
