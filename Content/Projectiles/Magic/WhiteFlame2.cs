using CalamityInheritance.Core;
using CalamityInheritance.Texture;
using CalamityInheritance.Utilities;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static CalamityInheritance.Utilities.CIFunction;
namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class WhiteFlameAltLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 64;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.5f / 255f, (255 - Projectile.alpha) * 0.5f / 255f, (255 - Projectile.alpha) * 0.5f / 255f);
            for (int i = 0; i < 5; i++)
            {
                int dType = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemDiamond, 0f, 0f, 100, default, 0.5f);
                Main.dust[dType].noGravity = true;
                Main.dust[dType].velocity *= 0.5f;
                Main.dust[dType].velocity += Projectile.velocity * 0.1f;
            }
            HomeInOnNPC(Projectile, true, 1800f, 16f, 5f);
        }
        private SpriteBatch SB { get => Main.spriteBatch; }
        private GraphicsDevice GD { get => Main.graphics.GraphicsDevice; }
        // public override bool PreDraw(ref Color lightColor)
        // {
        //     Texture2D sharpTears = TextureAssets.Extra[ExtrasID.SharpTears].Value;
        //     Color color = Color.White;
        //     Projectile.BaseProjPreDraw(sharpTears, color, Projectile.rotation + MathHelper.PiOver2);
        //     Projectile.BaseProjPreDraw(sharpTears, color, Projectile.rotation + 0f);
        //     BlendState defaultBlend = SB.GraphicsDevice.BlendState;
        //     SamplerState defaultSampler = SB.GraphicsDevice.SamplerStates[0];
        //     DrawVertex(Projectile.oldPos, defaultBlend, defaultSampler, Color.White);
        //     return false;
        // }
        // public void DrawVertex(Vector2[] oldP, BlendState defaultBlend, SamplerState defaultSampler, Color lightColor)
        // {
        //     //ban掉原本的绘制
        //     SB.End();
        //     //指定屏幕矩阵, 而非世界坐标
        //     SB.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullClockwise, null, Main.GameViewMatrix.TransformationMatrix);
        //     //干掉可能存在零向量
        //     List<Vector2> validPositions = [];
        //     List<float> validRot = [];
        //     for (int i = 0; i < oldP.Length; i++)
        //     {
        //         if (oldP[i] == Vector2.Zero)
        //             continue;

        //         validPositions.Add(oldP[i]);
        //         validRot.Add(Projectile.oldRot[i]);
        //     }
        //     int drawPointTime = 4;
        //     //自定义尝试获取贝塞尔曲线
        //     BaseBezierCurveInfo BezierCurve = GetValidBeizerCurvePow(validPositions, validRot, drawPointTime);
        //     List<Vector2> smoothPositions = BezierCurve.CurvePositionList;
        //     List<float> smoothRots = BezierCurve.CurveRotationList;
        //     List<CIVertexPositionColorTexture> vertexList = [];
        //     for (int i = 0; i < smoothPositions.Count; i++)
        //     {
        //         Vector2 worldCenter = smoothPositions[i] + Projectile.Size / 2f;
        //         Vector2 oldCenter = worldCenter - Main.screenPosition;
        //         float progress = (float)i / (smoothPositions.Count - 1);
        //         Vector2 posOffset = new Vector2(0, 12).RotatedBy(smoothRots[i] + MathHelper.PiOver4);
        //         Vector2 drawPos = new(oldCenter.X, oldCenter.Y);
        //         float alpha = 1f;
        //         //默认3顶点绘制方案
        //         if (i < drawPointTime || i > smoothPositions.Count - drawPointTime - 1)
        //             alpha = MathHelper.Lerp(0f, 1f, Math.Min(1f, (float)Math.Abs(i - drawPointTime) / drawPointTime));
        //         Color vertexColor = Color.White * alpha;
        //         CIVertexPositionColorTexture UpClass = new(drawPos + posOffset, vertexColor, new Vector3(progress, 0, 0));
        //         CIVertexPositionColorTexture DownClass = new(drawPos - posOffset, vertexColor, new Vector3(progress, 1, 0));
        //         vertexList.Add(UpClass);
        //         vertexList.Add(DownClass);
        //     }
        //     if (vertexList.Count >= 3)
        //     {
        //         //贴图
        //         GD.Textures[0] = CITextureRegistry.BaseTrail.Value;
        //         //绘制
        //         GD.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertexList.ToArray(), 0, vertexList.Count - 2);
        //     }
        //     SB.End();
        //     SB.Begin(SpriteSortMode.Deferred,
        //     defaultBlend,
        //     defaultSampler,
        //     DepthStencilState.None,
        //     RasterizerState.CullNone,
        //     null,
        //     Main.GameViewMatrix.TransformationMatrix);
        // }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 360);
        }
    }
}
