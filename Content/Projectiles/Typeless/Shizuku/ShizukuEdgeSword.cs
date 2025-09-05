using System;
using CalamityInheritance.Content.Items;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku
{
    public class ShizukuEdgeSword: ModProjectile, ILocalizedModType
    {
       public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public Player Owner => Main.player[Projectile.owner];
        public override string Texture => "CalamityInheritance/Content/Items/Weapons/Typeless/ShizukuItem/ShizukuEdge";
        public ref float AttackTimer => ref Projectile.ai[0];
        public ref float AttackType => ref Projectile.ai[1];
        public int[] GhostType =
            [
                ModContent.ProjectileType<SoulSmallPlaceholder>(),
                ModContent.ProjectileType<SoulMidPlaceholder>(),
                ModContent.ProjectileType<SoulLargePlaceholder>()
            ];
        #region AttackType
        const float IsShooted = 0f;
        const float IsHomingBack = 1f;
        #endregion
        #region Attack Args
        const float MaxShootAngles = 45;
        #endregion
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.height = 108;
            Projectile.width = 84;
            Projectile.scale *= 1.2f;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 90000;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.Opacity = 0f;
        }
        public override void AI()
        {
            SpiningAI();
            DoJustSpawn();
            // DoGeneral();
            float basicAttackSpeed = 30f;
            float getAttackSpeed = Owner.GetTotalAttackSpeed<MeleeDamageClass>();
            //将其转化为等比例攻速摸
            float speedMul = basicAttackSpeed / (1 + getAttackSpeed);
            //向下取整获得真正的攻速摸
            float actualSpeed = Math.Max(1, speedMul);
            if (AttackTimer % basicAttackSpeed == 0)
            {
                ShootGhosts();
            }
            //飞剑的发射是固定模
            if (AttackTimer % 50f == 0)
            {
                ShootSwords();
            }
            // switch (AttackType)
            // {
            //     case IsShooted:
            //         DoShooted();
            //         break;
            //     case IsHomingBack:
            //         DoHomingBack();
            //         break;
            // }
        }

        public void ShootGhosts()
        {
            int baseCount = 3;
            int realCount = baseCount + (Owner.maxMinions + Owner.maxTurrets) / 2;
            Vector2 shootDirection = Main.MouseWorld - Owner.Center;
            //将其标准化
            shootDirection.Normalize();
            //获取实际散射范围与固定的间隔
            float angleStep = MaxShootAngles * 2 / (realCount - 1);
            for (int i = 0; i < realCount; i++)
            {
                //每次执行随机选择的鬼魂
                int projID = Utils.SelectRandom(Main.rand, GhostType);
                float angleStepOffset = MathHelper.ToRadians(-angleStep + (i * angleStep));
                Vector2 spreadDirection = RotateVector(shootDirection, angleStepOffset);
                //获取实际速度
                Vector2 spreadVelocity = spreadDirection * 16f;
                //发射鬼魂
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, spreadVelocity, projID, Projectile.damage, Projectile.knockBack, Owner.whoAmI, ShizukuBaseGhost.MoreTrailingDust);
            }
        }
        //旋转向辅助方法
        public static Vector2 RotateVector(Vector2 vector, float angle)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            return new Vector2(vector.X * cos - vector.Y * sin, vector.X * sin + vector.Y * cos);
        }
        private void SpiningAI()
        {
            float spinTime = 50f;
            if (Owner.dead || !Owner.channel)
            {
                // Main.NewText("FuckedMoon, Velocity:" + Projectile.velocity.Length().ToString());
                Projectile.Kill();
                Owner.reuseDelay = 2;
                return;
            }
            int spinDir = Math.Sign(Projectile.velocity.X);
            Projectile.velocity = new Vector2(spinDir, 0f);
            if (AttackTimer == 0f)
            {
                Projectile.rotation = new Vector2(Projectile.velocity.X, -Owner.gravDir).ToRotation() + MathHelper.ToRadians(135f);
                if (Projectile.velocity.X < 0f)
                {
                    Projectile.rotation -= MathHelper.PiOver2;
                }
            }
            AttackTimer += 1f;
            Projectile.rotation += MathHelper.TwoPi * 2f / spinTime * spinDir;
            int wantedDire = (Owner.SafeDirectionTo(Main.MouseWorld).X > 0f).ToDirectionInt();
            if (AttackTimer % spinTime > spinTime * 0.5f && wantedDire != Projectile.velocity.X)
            {
                Owner.ChangeDir(wantedDire);
                Projectile.velocity = Vector2.UnitX * wantedDire;
                Projectile.rotation -= MathHelper.Pi;
                Projectile.netUpdate = true;
            }
            PosAndRot();
        }
        public void PosAndRot()
        {
            Vector2 ifPlayerControl = Owner.RotatedRelativePoint(Owner.MountedCenter, true);
            Projectile.Center = new Vector2(ifPlayerControl.X, ifPlayerControl.Y);
            Projectile.spriteDirection = Projectile.direction;
            Projectile.timeLeft = 2;
            Owner.ChangeDir(Projectile.direction);
            Owner.heldProj = Projectile.whoAmI;
            Owner.itemTime = Owner.itemAnimation = 2;
            Owner.itemRotation = MathHelper.WrapAngle(Projectile.rotation);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }
            float spinning = Projectile.rotation - MathHelper.PiOver4 * (float)Math.Sign(Projectile.velocity.X);
            float staffRadiusHit = 110f;
            float useless = 0f;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + spinning.ToRotationVector2() * -staffRadiusHit, Projectile.Center + spinning.ToRotationVector2() * staffRadiusHit, 23f * Projectile.scale, ref useless))
            {
                return true;
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            //手动接管绘制
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Rectangle rec = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 ori = texture.Size() / 2f;
            Color color = Color.White;
            SpriteEffects sE = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                sE = SpriteEffects.FlipHorizontally;
            Main.EntitySpriteDraw(texture, drawPos, new Rectangle?(rec), color, Projectile.rotation, ori, Projectile.scale, sE, 0);
            return false;
        }
        private void ShootSwords()
        {
            if (AttackTimer % 30 != 0)
                return;
            
            int count = 2;
            int proj = ModContent.ProjectileType<ShizukuSwordProjectile>();
            for (int i = -1; i < count; i += 2)
            {
                //获得指针与玩家的位置并归一化
                Vector2 direction = Main.MouseWorld - Owner.Center;
                direction.Normalize();
                Vector2 setSpeed = direction.RotatedBy(MathHelper.PiOver4 / 1.5f * i) * -24f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, setSpeed * 1.1f, proj, Projectile.damage, Projectile.knockBack, Owner.whoAmI, ShizukuSwordProjectile.ShouldSlash);
            }
        }
        private void DoJustSpawn()
        {
            //生成时的粒子
            if (AttackTimer == -1f)
            {
                SoundEngine.PlaySound(CISoundID.SoundIceRodBlockPlaced, Projectile.Center);
                PlayDust();
            }
        }

        private void DoShooted()
        {
            ShootSwords();

            AttackTimer += 1f;
            Projectile.Opacity += 0.1f;
            if (AttackTimer < 35f)
                return;

            AttackType = IsHomingBack;
            AttackTimer = 0f;
            Projectile.netUpdate = true;
        }

        private void DoHomingBack()
        {
            Projectile.velocity.X *= 0.9f;
            Projectile.rotation += 0.05f;
            Projectile.Opacity -= 0.1f;
            if (Projectile.Opacity < 0.5f)
            {
                AttackTimer += 1f + (AttackTimer < 15f).ToInt();
                Projectile.velocity.Y = -AttackTimer;
                if (Projectile.Opacity == 0f)
                {
                    Projectile.Kill();
                    Projectile.netUpdate = true;
                }
            }
        }

        public void PlayDust()
        {
            //一圈冰系粒子，受重力
            int dType = Main.rand.NextBool() ? DustID.IceRod : DustID.IceTorch;
            CIFunction.DustCircle(Projectile.Center, 32f, Main.rand.NextFloat(0.8f, 1.21f), dType, true, 12f, 200);
        }
    } 
}