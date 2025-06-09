using CalamityInheritance.Content.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod;
using static CalamityInheritance.Utilities.CIFunction;
using static tModPorter.ProgressUpdate;
using CalamityInheritance.Content.Projectiles.Ranged;
using Mono.Cecil;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using CalamityInheritance.System.Configs;
using CalamityMod.Particles;
using CalamityMod.NPCs.NormalNPCs;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Ranged
{
    public class PearlGodLegacyHeldProj : BaseHeldProj, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public float rotProg;
        public override float OffsetX => -21;
        public override float OffsetY => 0;
        public override float BaseOffsetY => 5;
        public override float WeaponRotation => rotProg;
        private const int defaultSpread = 1;
        private int spread = defaultSpread;
        private bool finalShot = false;
        // 旋转速度
        public override float AimResponsiveness => 0.2f;
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 46;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.scale = 0.75f;
        }
        public override void HoldoutAI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.extraUpdates = 0;
            // 用于储存动画进度
            ref float AniProgress = ref Projectile.ai[0];

            if (AniProgress == 0 && !Main.mouseRight)
            {
                SoundEngine.PlaySound(SoundID.Item41, Projectile.Center);
                CustomShoot();
            }
            AniProgress++;
            RecoilAnimation(ref AniProgress);
        }
        #region 后坐力动画
        public void RecoilAnimation(ref float AniProgress)
        {
            Player player = Main.player[Projectile.owner];
            if(Main.mouseRight)
            {
                rotProg = MathHelper.Lerp(rotProg, 0f, 0.25f);
                return;
            }
            // 总进度
            int recoilani = Owner.HeldItem.useTime;
            int Halfrecoilani = Owner.HeldItem.useTime / 2;
            // 旋转最终加值
            float rotProgress;
            if (AniProgress <= Halfrecoilani)
            {
                float progress = EasingHelper.EaseInOutQuad((float)AniProgress / Halfrecoilani);
                // 最多转7度
                rotProgress = MathHelper.Lerp(0, 7, progress);
                rotProg = -rotProgress;
            }
            else if (AniProgress > Halfrecoilani && AniProgress < recoilani)
            {
                rotProg = MathHelper.Lerp(rotProg, 0f, 0.25f);
            }
            else
            {
                AniProgress = 0;
                rotProg = 0;
            }
        }
        #endregion
        #region 发射逻辑
        public void CustomShoot()
        {
            if (spread > 6)
            {
                spread = defaultSpread;
                finalShot = true;
            }
            rotProg = 0;
            var source = Projectile.GetSource_FromThis();
            float rotation = MathHelper.ToRadians(spread);

            Vector2 firedirection = Vector2.UnitX.RotatedBy(Projectile.rotation);
            firedirection = firedirection.SafeNormalize(Vector2.UnitX);

            Owner.PickAmmo(Owner.ActiveItem(), out int Proj, out float shootSpeed, out int damage, out float knockback, out _, false);
            Vector2 velocity = firedirection * shootSpeed;

            Vector2 offset = new Vector2(55, -5 * Projectile.direction).RotatedBy(Projectile.rotation);
            Vector2 firepos = Projectile.Center + offset;
            if (!finalShot)
            {
                int totalLoops = 1;
                switch (spread)
                {
                    case 1:
                    case 2:
                        break;
                    case 3:
                    case 4:
                        totalLoops = 2;
                        break;
                    case 5:
                    case 6:
                        totalLoops = 3;
                        break;
                }
                for (int i = 0; i < totalLoops; i++)
                {
                    int bullet1 = Projectile.NewProjectile(source, firepos, velocity.RotatedBy(-rotation * (i + 1)), Proj, (int)(damage * 0.5), knockback * 0.5f, Owner.whoAmI);
                    Main.projectile[bullet1].extraUpdates += spread;
                    int bullet2 = Projectile.NewProjectile(source, firepos, velocity.RotatedBy(+rotation * (i + 1)), Proj, (int)(damage * 0.5), knockback * 0.5f, Owner.whoAmI);
                    Main.projectile[bullet2].extraUpdates += spread;
                }

                int shockblast = Projectile.NewProjectile(source, firepos, velocity, ModContent.ProjectileType<ShockblastRoundLegacy>(), damage, knockback, Owner.whoAmI, 0f, spread);
                Main.projectile[shockblast].extraUpdates += spread;

                spread++;

                for (int k = 0; k < 2; k++)
                {
                    int randomColor = Main.rand.Next(1, 2 + 1);
                    Color color = randomColor == 1 ? Color.LightBlue :Color.Khaki;
                    SparkParticle spark = new SparkParticle(firepos, velocity.RotatedByRandom(0.25) * Main.rand.NextFloat(0.2f, 1.5f), false, Main.rand.Next(20, 25 + 1), Main.rand.NextFloat(0.4f, 0.65f), color);
                    GeneralParticleHandler.SpawnParticle(spark);
                }
                for (int k = 0; k < 6; k++)
                {
                    int randomColor = Main.rand.Next(1, 2 + 1);
                    Color color = randomColor == 1 ? Color.LightBlue : Color.Khaki;
                    PearlParticle pearl1 = new PearlParticle(firepos, velocity.RotatedByRandom(0.25) * Main.rand.NextFloat(0.2f, 1f), false, Main.rand.Next(40, 45 + 1), Main.rand.NextFloat(0.6f, 0.75f), color, 0.95f, Main.rand.NextFloat(1, -1), true);
                    GeneralParticleHandler.SpawnParticle(pearl1);
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    int bigShockblast = Projectile.NewProjectile(source, Projectile.Center + offset, velocity, ModContent.ProjectileType<ShockblastRoundLegacy>(), damage * 2, knockback * 2f, Owner.whoAmI, 0f, 10f);
                    Main.projectile[bigShockblast].extraUpdates += 9;
                }

                for (int k = 0; k < 2; k++)
                {
                    int randomColor = Main.rand.Next(1, 3 + 1);
                    Color color = randomColor == 1 ? Color.LightBlue : Color.Khaki;
                    SparkParticle spark = new SparkParticle(firepos, velocity.RotatedByRandom(0.25) * Main.rand.NextFloat(0.2f, 1.5f), false, Main.rand.Next(20, 25 + 1), Main.rand.NextFloat(0.4f, 0.65f), color);
                    GeneralParticleHandler.SpawnParticle(spark);
                }
                for (int k = 0; k < 6; k++)
                {
                    int randomColor = Main.rand.Next(1, 3 + 1);
                    Color color = randomColor == 1 ? Color.LightBlue : Color.Khaki;
                    PearlParticle pearl1 = new PearlParticle(firepos, velocity.RotatedByRandom(0.25) * Main.rand.NextFloat(0.2f, 1f), false, Main.rand.Next(40, 45 + 1), Main.rand.NextFloat(0.6f, 0.75f), color, 0.95f, Main.rand.NextFloat(1, -1), true);
                    GeneralParticleHandler.SpawnParticle(pearl1);
                }

                finalShot = false;
            }
        }
        #endregion
        public float blinkTimer;
        public override bool ExtraPreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            #region 闪烁效果
            blinkTimer += Main.rand.NextFloat(0.15f, 0.45f);
            // 范围100-200的透明度变化
            float alphaFactor = (float)(Math.Sin(blinkTimer) + 1) / 2; // 转换为0-1范围，而不是-1到1范围
            int alpha = 125 + (int)(75 * alphaFactor); // 映射到125-200范围
            Color blinkColor = new Color(255, 255, 255, alpha);
            #endregion
            #region 材质注册
            Texture2D laserTexture = ModContent.Request<Texture2D>("CalamityInheritance/ExtraTextures/WeaponsTextures/PearlGodAimLaser").Value;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Texture2D Glowtexture = ModContent.Request<Texture2D>("CalamityInheritance/Content/Projectiles/HeldProj/Ranged/PearlGodLegacyHeldProjGlow").Value;
            #endregion

            Vector2 offset = new Vector2(10, 7 * player.direction).RotatedBy(Projectile.rotation);
            Vector2 drawPosition = Projectile.Center + offset - Main.screenPosition;

            Vector2 orig = new(0, laserTexture.Height / 2);
            SpriteEffects flipSprite = (player.direction * Main.player[Projectile.owner].gravDir == -1) ? SpriteEffects.None : SpriteEffects.FlipVertically;

            // 保存原始混合状态
            var originalBlendState = Main.spriteBatch.GraphicsDevice.BlendState;

            // 重置绘制批次来设置叠加混合模式
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(laserTexture, drawPosition, null, blinkColor, Projectile.rotation, orig, Projectile.scale * Main.player[Projectile.owner].gravDir * 0.5f, flipSprite);

            // 绘制后恢复原始状态
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, originalBlendState, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Vector2 Baseorig = new(0, texture.Height / 2);

            Vector2 drawBasePosition = (Projectile.Center) - Main.screenPosition;
            SpriteEffects BaseflipSprite = (player.direction * Main.player[Projectile.owner].gravDir == -1) ? SpriteEffects.FlipVertically : SpriteEffects.None;
            Main.EntitySpriteDraw(texture, drawBasePosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, Baseorig, Projectile.scale * Main.player[Projectile.owner].gravDir, BaseflipSprite);
            Main.EntitySpriteDraw(Glowtexture, drawBasePosition, null, Color.White, Projectile.rotation, Baseorig, Projectile.scale * Main.player[Projectile.owner].gravDir, BaseflipSprite);
            return false;
        }
    }
}
