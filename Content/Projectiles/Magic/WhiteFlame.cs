using System;
using System.Collections.Generic;
using CalamityInheritance.Core;
using CalamityInheritance.Texture;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static CalamityInheritance.Utilities.CIFunction;
namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class WhiteFlameLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";

        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 50;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Player projOwner = Main.player[Projectile.owner];
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.5f / 255f, (255 - Projectile.alpha) * 0.5f / 255f, (255 - Projectile.alpha) * 0.5f / 255f);
            for (int i = 0; i < 1; i++)
            {
                int dWhite = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemDiamond, 0f, 0f, 100, default, 1.2f);
                Main.dust[dWhite].noGravity = true;
                Main.dust[dWhite].velocity *= 0.5f;
                Main.dust[dWhite].velocity += Projectile.velocity * 0.1f;
                Main.dust[dWhite].scale = 0.8f;
            }
            float homingRange = 1000f;
            Projectile.localAI[0] += 1f;
            if (projOwner.active && !projOwner.dead)
            {
                if (Projectile.Distance(projOwner.Center) > homingRange)
                {
                    Projectile.SafeDirectionTo(projOwner.Center, Vector2.UnitX);
                    return;
                }
                if (Projectile.localAI[0] > 10f)
                    HomeInOnNPC(Projectile, true, 700f, 16f, 20f);
            }
            else
            {
                if (Projectile.timeLeft > 30)
                    Projectile.timeLeft = 30;
            }
        }
        private SpriteBatch SB { get => Main.spriteBatch; }
        private GraphicsDevice GD { get => Main.graphics.GraphicsDevice; }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sharpTears = TextureAssets.Extra[ExtrasID.SharpTears].Value;
            Color color = Color.White;
            Projectile.BaseProjPreDraw(sharpTears, color, Projectile.rotation + MathHelper.PiOver2);
            Projectile.BaseProjPreDraw(sharpTears, color, Projectile.rotation + 0f);
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
                Vector2 posOffset = new Vector2(0, 12).RotatedBy(smoothRots[i] + MathHelper.PiOver4);
                Vector2 drawPos = new(oldCenter.X, oldCenter.Y);
                Color vertexColor = Color.White;
                CIVertexPositionColorTexture UpClass = new(drawPos + posOffset, vertexColor, new Vector3(progress, 0, 0));
                CIVertexPositionColorTexture DownClass = new(drawPos - posOffset, vertexColor, new Vector3(progress, 1, 0));
                vertexList.Add(UpClass);
                vertexList.Add(DownClass);
            }
            if (vertexList.Count >= 3)
            {
                //贴图
                GD.Textures[0] = CITextureRegistry.BaseTrail.Value;
                //绘制
                GD.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertexList.ToArray(), 0, vertexList.Count - 2);
            }
            SB.End();
            SB.Begin(SpriteSortMode.Deferred,
BlendState.AlphaBlend, SamplerState.PointClamp,
            DepthStencilState.None,
            RasterizerState.CullNone,
            null,
            Main.GameViewMatrix.TransformationMatrix);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.Item122, Projectile.position);
            int summonCounts = 2;
            if (Projectile.owner == Main.myPlayer)
            {
                for (int j = 0; j < summonCounts; j++)
                {
                    Vector2 speed = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                    while (speed.X == 0f && speed.Y == 0f)
                    {
                        speed = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                    }
                    speed.Normalize();
                    speed *= Main.rand.Next(70, 101) * 0.1f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(),Projectile.oldPosition.X + Projectile.width / 2, Projectile.oldPosition.Y + Projectile.height / 2, speed.X, speed.Y, ModContent.ProjectileType<WhiteFlameAltLegacy>(), (int)(double)Projectile.damage, 0f, Projectile.owner, 0f, 0f);
                }
            }
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 360);
        }
    }
}
