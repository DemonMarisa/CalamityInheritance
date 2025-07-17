using System;
using System.Collections.Generic;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Content.Items.Weapons;
using CalamityInheritance.Content.Items.Weapons.Typeless;
using CalamityInheritance.Core;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku
{
    public class ShizukuSwordProjectile : ModProjectile
    {
        public Player Owner => Main.player[Projectile.owner];
        public override string Texture => $"{Generic.WeaponPath}/Typeless/ShizukuItem/ShizukuSword";
        //目前没有作用，我认为应该去掉。
        public int TargetIndex
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public ref float AttackTimer => ref Projectile.ai[1];
        public ref float AttackType => ref Projectile.ai[2];
        #region AttackType
        const float IsSpawning = 0f;
        const float IsAngleTo = 1f;
        const float IsDashing = 2f;
        #endregion
        #region 顶点绘制使用
        public float totalOldPos = 20;
        public static Asset<Texture2D> TrailTexture;
        #endregion

        public override void SetStaticDefaults()
        {
            TrailTexture = ModContent.Request<Texture2D>($"{Generic.ProjPath}/Typeless/Shizuku/ShizukuSword_Trail");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = (int)totalOldPos;
        }
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 112;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.friendly = true;
            Projectile.timeLeft = 18000;
            Projectile.tileCollide = false;
            Projectile.Opacity = 0f;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }
        public override bool? CanDamage()
        {
            return AttackType == IsDashing;
        }
        public override void AI()
        {
            //暂时禁用，这个射弹最后会和Shizuku Edge融为一体
            // CheckHeldItem();
            NPC anyTarget = Projectile.FindClosestTarget(CIFunction.SetDistance(100));
            switch (AttackType)
            {
                case IsSpawning:
                    DoSpawning(anyTarget);
                    break;
                case IsAngleTo:
                    DoAngleTo(anyTarget);
                    break;
                case IsDashing:
                    DoDashing(anyTarget);
                    break;
            }
        }
        //拖尾粒子
        private void TrailingDust()
        {
            if (Main.rand.NextBool(4))
            {
                int dustType = Main.rand.NextBool(5) ? 226 : 220;
                float scale = 0.8f + Main.rand.NextFloat(0.3f);
                float velocityMult = Main.rand.NextFloat(0.3f, 0.6f);
                int idx = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity = velocityMult * Projectile.velocity;
                Main.dust[idx].scale = scale;
            }
        }

        private void DoDashing(NPC anyTarget)
        {
            if (anyTarget is null)
                return;
            //dashing.
            TrailingDust();
            AttackTimer++;
            float homingDistance = CIFunction.SetDistance(200);
            Projectile.HomingNPCBetter(anyTarget, homingDistance, 20f + AttackTimer / 10f, 20f, 2);
        }

        private void DoAngleTo(NPC anyTarget)
        {
            if (anyTarget is null)
                return;
            float shouldAngleTo = Projectile.AngleTo(anyTarget.Center) + MathHelper.PiOver4;
            //这个没生效。
            if (Projectile.rotation == shouldAngleTo)
                AttackTimer = 25f;
            
            Projectile.rotation = Utils.AngleLerp(Projectile.rotation, shouldAngleTo, 0.15f);
            AttackTimer++;
            if (AttackTimer > 25f)
            {
                Projectile.netUpdate = true;
                AttackType = IsDashing;
                AttackTimer = 0f;
            }
        }

        private void DoSpawning(NPC anyTarget)
        {
            //减速
            Projectile.velocity *= 0.92f;
            //渐变
            Projectile.Opacity += 0.05f;
            if (anyTarget is not null)
            {
                float shouldAngleTo = Projectile.AngleTo(anyTarget.Center) + MathHelper.PiOver4;
                Projectile.rotation = Utils.AngleLerp(Projectile.rotation, shouldAngleTo, 0.10f);
            }
            else Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            TrailingDust();
            //查阅速度与Opacity是否达到要求。
            if (Projectile.Opacity >= 1 && Projectile.velocity.Length() <= 0.4f)
            {
                Projectile.netUpdate = true;
                AttackType = IsAngleTo;
            }
        }

        public void DoAttacking()
        {
            NPC target = Main.npc[TargetIndex];

        }

        public void IdlePosition()
        {

        }

        public void CheckHeldItem()
        {
            //if (Owner.HeldItem.type != ModContent.ItemType<ShizukuEdge>())
            //{
            //    Projectile.Kill();
            //}
        }
        public SpriteBatch spriteBatch { get => Main.spriteBatch; }
        public GraphicsDevice graphicsDevice { get => Main.graphics.GraphicsDevice; }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Projectile.BaseProjPreDraw(texture, lightColor, MathHelper.ToRadians(7));

            Vector2[] oldPos = Projectile.oldPos;

            spriteBatch.End();
            spriteBatch.Begin(
                SpriteSortMode.Immediate, 
                BlendState.Additive,
                SamplerState.AnisotropicClamp,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null,
                Main.GameViewMatrix.TransformationMatrix);// 屏幕矩阵

            List<CIVertexPositionColorTexture> Vertexlist = [];

            var halftextureWidth = texture.Width / 2;
            var halftextureHeight = texture.Height / 2;

            for (int i = 0; i < oldPos.Length; i++)
            {
                Vector2 DrawPos = new Vector2(oldPos[i].X + halftextureWidth, oldPos[i].Y + halftextureHeight) - Main.screenPosition;
                // 如果第一个点是0,0，那么会飞到世界远点，会画一条巨远的线，所以这里要判断一下。
                if (oldPos[i] != Vector2.Zero)
                {
                    // DrawPos.Y - halftextureHeight 这减用于确定一个片的Y高度，不一定必须要是贴图的一半，也可以是任意数值
                    // 贴图的正上方
                    Vertexlist.Add(new CIVertexPositionColorTexture(
                        position: new Vector2(DrawPos.X, DrawPos.Y - 40),
                        color: Color.White,
                        // 将一个片平均分成20份，这里是上半部分的点
                        textureCoordinate: new Vector3(i / totalOldPos, 0, 1)
                        ));

                    // 贴图的正上方
                    Vertexlist.Add(new CIVertexPositionColorTexture(
                        position: new Vector2(DrawPos.X, DrawPos.Y + 40),
                        color: Color.White,
                        // 将一个片平均分成20份，这里是下半部分的点
                        textureCoordinate: new Vector3(i / totalOldPos, 0, 1)
                        ));
                }
            }

            // 因为至少有三个点才可以绘制，所以这里要判断一下。
            if (Vertexlist.Count > 3)
            {
                graphicsDevice.Textures[0] = TrailTexture.Value;
                graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, Vertexlist.ToArray(), 0, Vertexlist.Count / 2);
            }

            spriteBatch.End();
            spriteBatch.Begin();
            return false;
        }
    }
}