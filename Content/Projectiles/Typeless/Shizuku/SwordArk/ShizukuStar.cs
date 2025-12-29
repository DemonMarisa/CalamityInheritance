using System;
using System.Collections.Generic;
using CalamityInheritance.Buffs.Statbuffs;
using CalamityInheritance.Core;
using CalamityInheritance.ExtraTextures.Metaballs;
using CalamityInheritance.Texture;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static CalamityInheritance.Utilities.CIFunction;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku.SwordArk
{
    public class ShizukuStar : ModProjectile,ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public Player Owner => Main.player[Projectile.owner];
        public int AttackTimer
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public Vector2 _cacheStartPosition = Vector2.Zero;
        //重染色的鸿蒙之星
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 32;
            Projectile.extraUpdates = 2;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.penetrate = 4;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
            Projectile.noEnchantmentVisuals = true;
            Projectile.timeLeft = 400;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new(172, 236, 255, 75);
        }
        public override void AI()
        {
            
            if (AttackTimer is 0)
            {
                //像粒子一样绘制Metaball，这里粒绘制方法类似于锤子的粒子
                Vector2 offset = Projectile.Center;
                _cacheStartPosition = offset;
                //生成“粒子”
                for (int i = 0; i < 8; i++)
                {
                    float scale = MathHelper.Lerp(18f, 26f, CalamityUtils.Convert01To010(i / 12f));
                    offset = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.UnitY) * MathHelper.Lerp(-40f, 40f, i / 12f);
                    Vector2 particleVelocity = Projectile.velocity.SafeNormalize(Vector2.UnitY).RotatedByRandom(0.25f) * Main.rand.NextFloat(2.5f, 3.5f);
                    ShizukuStarMetaball.SpawnParticle(offset, particleVelocity, scale);
                }
            }
            DrawDust();
            if (AttackTimer < 10)
                ShizukuStarMetaball.SpawnParticle(_cacheStartPosition, Vector2.Zero, 54f);
            AttackTimer += 1;
            Projectile.rotation = Projectile.velocity.ToRotation();
            //追踪敌怪即可，
            NPC target = Projectile.FindClosestTarget(1800f);
            if (target != null && AttackTimer > 10f)
                Projectile.HomingNPCBetter(target, 1f, 20f, 20f, ignoreDist: true);
            
        }

        private void DrawDust()
        {
            if (Main.rand.NextBool(4))
            {
                Dust d = Dust.NewDustDirect(Projectile.Center, 24, 24, Main.rand.NextBool() ? DustID.PortalBoltTrail : DustID.PortalBolt, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f);
                d.scale = Main.rand.NextFloat(0.8f, 1.2f);
                d.color = new(91, 222, 255);
                d.noGravity = true;
            }
        }
        #region 地形效果
        #endregion
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<ShizukuMoonlight>(), 600);
            if (Projectile.DamageType == DamageClass.Magic)
            {
                target.CIMod().moonClass = ShizukuMoonlight.ClassType.Magic;
                Owner.CIMod().moonClass = ShizukuMoonlight.ClassType.Magic;
            }
            if (Projectile.DamageType == DamageClass.Melee)
            {
                target.CIMod().moonClass = ShizukuMoonlight.ClassType.Melee;
                Owner.CIMod().moonClass = ShizukuMoonlight.ClassType.Melee;
            }

            Owner.AddBuff(BuffType<ShizukuMoonlight>(), 60);
        }
        public override bool PreKill(int timeLeft)
        {
            if (Projectile.DamageType == DamageClass.Melee)
                HandleDefault();
            else
                DrawAquaStarParticle();
            return true;
        }
        private void HandleDefault()
        {
            //树妖增防
            Owner.AddBuff(BuffID.DryadsWard, 600);
            Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ProjectileID.TerrarianBeam, Projectile.damage / 2, Projectile.knockBack, Owner.whoAmI);
            proj.DamageType = DamageClass.Generic;
        }
        private void DrawAquaStarParticle()
        {
            //绘制3个，扇形散射
            int count = 3;
            float fanAngle = MathHelper.ToRadians(80f);
            float angleStep = fanAngle / (count - 1);
            float startAngle = Projectile.rotation - fanAngle / 2f;
            for (int i = 0; i < count; i++)
            {
                float curAngle = startAngle + Main.rand.NextFloat(MathHelper.ToRadians(5), MathHelper.ToRadians(15)) + angleStep * i;
                Vector2 vel = curAngle.ToRotationVector2() * Projectile.velocity.Length();
                float scale = Main.rand.NextFloat(1f, 1.5f);
                int lifeTime = Main.rand.Next(30, 60);
                CuteManaStarParticle manaStar = new(Projectile.Center, vel, scale, 1, lifeTime);
                GeneralParticleHandler.SpawnParticle(manaStar);
            }
        }
        //这里用Metaball代替了绘制。
        private SpriteBatch SB { get => Main.spriteBatch; }
        private GraphicsDevice GD { get => Main.graphics.GraphicsDevice; }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Projectile.BaseProjPreDraw(tex, 8, Color.White);
            DrawVertex(Projectile.oldPos, Color.White);
            return false;
        }
        public void DrawVertex(Vector2[] oldP, Color lightColor)
        {
            //ban掉原本的绘制
            SB.End();
            //指定屏幕矩阵, 而非世界坐标
            SB.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            //干掉可能存在零向量
            List<Vector2> validPositions = [];
            List<float> validRot = [];
            for (int i = 0; i < oldP.Length; i++)
            {
                if (oldP[i] == Vector2.Zero)
                    continue;

                validPositions.Add(oldP[i]);
                validRot.Add(Projectile.oldRot[i]);
            }
            int drawPointTime = 4;
            //自定义尝试获取贝塞尔曲线
            BaseBezierCurveInfo BezierCurve = GetValidBeizerCurvePow(validPositions, validRot, drawPointTime);
            List<Vector2> smoothPositions = BezierCurve.CurvePositionList;
            List<float> smoothRots = BezierCurve.CurveRotationList;
            List<CIVertexPositionColorTexture> vertexList = [];
            for (int i = 0; i < smoothPositions.Count; i++)
            {
                Vector2 worldCenter = smoothPositions[i] + Projectile.Size / 2f;
                Vector2 oldCenter = worldCenter - Main.screenPosition;
                float progress = (float)i / (smoothPositions.Count - 1);
                Vector2 posOffset = new Vector2(0, 12).RotatedBy(smoothRots[i]);
                Vector2 drawPos = new(oldCenter.X, oldCenter.Y);
                float alpha = 1f;
                //默认3顶点绘制方案
                if (i < drawPointTime || i > smoothPositions.Count - drawPointTime - 1)
                    alpha = MathHelper.Lerp(0f, 1f, Math.Min(1f, (float)Math.Abs(i - drawPointTime) / drawPointTime));
                Color vertexColor = Color.SkyBlue * alpha;
                CIVertexPositionColorTexture UpClass = new(drawPos + posOffset, vertexColor, new Vector3(progress, 0, 0));
                CIVertexPositionColorTexture DownClass = new(drawPos - posOffset, vertexColor, new Vector3(progress, 1, 0));
                vertexList.Add(UpClass);
                vertexList.Add(DownClass);
            }
            if (vertexList.Count >= 3)
            {
                //贴图
                GD.Textures[0] = CITextureRegistry.ShizukuArkTrail.Value;
                //绘制
                GD.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertexList.ToArray(), 0, vertexList.Count - 2);
            }
            SB.End();
            SB.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
            DepthStencilState.None,
            RasterizerState.CullNone,
            null,
            Main.GameViewMatrix.TransformationMatrix);
        }
    }
}