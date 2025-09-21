using System;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Xml.Linq;
using CalamityInheritance.Particles;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku.SwordArk
{
    
    public class ShizukuDagger : ModProjectile, ILocalizedModType
    {
        private enum Style
        {
            IsShooted,
            IsAngleTo,
            IsFlying,
            DropOut
        }
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public Player Owner => Projectile.GetProjOwner();
        public ref float AttackTimer => ref Projectile.ai[0];
        public int TargetIndex
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        private Style DoType
        {
            get =>(Style)Projectile.ai[2];
            set => Projectile.ai[2] = (float)value;
        }
        public ref float DashingTimer => ref Projectile.CalamityInheritance().ProjNewAI[0];
        private int ShouldHoming = 0;
        private const float SpiningTime = 45;
        private const float ShootTime = 25; 
        public override void SetStaticDefaults() => ProjectileID.Sets.CultistIsResistantTo[Type] = true;
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 150;
            Projectile.MaxUpdates = 2;
            Projectile.alpha = 0;
            Projectile.tileCollide = false;
            Projectile.Opacity = 0f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = Projectile.MaxUpdates * 15;
            Projectile.DamageType = DamageClass.Generic;
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.DoSyncHandlerRead(ref reader);
            DashingTimer = reader.ReadSingle();
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            Projectile.DoSyncHandlerWrite(ref writer);
            writer.Write(DashingTimer);
        }
        public override bool? CanDamage() => DoType != Style.IsShooted;
        public override void AI()
        {
            NPC target = Main.npc[TargetIndex];
            if (target.CanBeChasedBy(Projectile) || !target.active)
                target = Projectile.FindClosestTarget(1800f);
            DashingTimer += 1;
            Projectile.Opacity = Utils.GetLerpValue(0f, 1f, DashingTimer, true) * Utils.GetLerpValue(0f, 1f, Projectile.timeLeft, true);
            //缓动，减速，与自转，类似于隔壁ACT射弹
            switch (DoType)
            {
                case Style.IsShooted:
                    DoShooted(target);
                    break;
                case Style.IsAngleTo:
                    DoAngleTo(target);
                    break;
                case Style.IsFlying:
                    DoFlying(target);
                    break;
                case Style.DropOut:
                    Droppper();
                    break;
            }
        }

        private void Droppper()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            DrawSparkLine();
        }

        private void DrawSparkLine()
        {
            //日食矛抄过来的
            if (Projectile.Opacity > 0.5f)
            {
                if (Main.rand.NextBool(5))
                {
                    Vector2 trailPos = Projectile.Center + Vector2.UnitY.RotatedBy(Projectile.rotation) * Main.rand.NextFloat(-8f, 8f);
                    float trailScale = Main.rand.NextFloat(0.8f, 1.0f);
                    Color trailColor = new(40, 171, 231);
                    Particle eclipseTrail = new SparkParticle(trailPos, Projectile.velocity * 0.2f, false, 60, trailScale, trailColor);
                    Particle eclipseTrai2 = new StarProjBlack(trailPos, Projectile.velocity * 0.2f, false, 60, trailScale, Color.GhostWhite);
                    GeneralParticleHandler.SpawnParticle(Main.rand.NextBool() ? eclipseTrail : eclipseTrai2);
                }
            }
        }

        private void DoFlying(NPC target)
        {
            //查看敌对单是否不可用或者为空
            bool shouldPointToMouse = target is null || !target.CanBeChasedBy(Projectile) || !target.active;
            //如果符合，设定射弹初始为指向指针的速度。且不再变化
            DrawSparkLine();
            if (shouldPointToMouse)
            {
                if (Projectile.timeLeft > 50)
                    Projectile.timeLeft = 50;
                if (AttackTimer is 0)
                {
                    Vector2 direction = (Projectile.Center - Main.MouseWorld).SafeNormalize(Vector2.UnitX);
                    Projectile.velocity = direction * 23;
                    Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver4;
                    AttackTimer = -1;
                }
            }
            //敌对单位如果可用，则正常冲向敌对单位
            else if (AttackTimer != -1 && !shouldPointToMouse)
            {
                if (AttackTimer is 0)
                {
                    float angleTo = Projectile.AngleTo(target.Center) + MathHelper.PiOver4;
                    Projectile.rotation = angleTo;
                    Projectile.velocity = (angleTo - MathHelper.PiOver4).ToRotationVector2() * 20f; 
                }
                AttackTimer += 1;
                if (AttackTimer > 15)
                {
                    Projectile.HomingNPCBetter(target, 18f + AttackTimer / 2, 20f, 1);
                    Projectile.rotation = MathHelper.Lerp(Projectile.rotation,Projectile.velocity.ToRotation() - MathHelper.PiOver4, 0.2f);
                }
            }
        }
        private void DoAngleTo(NPC target)
        {
            //指向你的敌人。
            Vector2 tar = target is null || !target.CanBeChasedBy(Projectile) || !target.active ? Main.MouseWorld : target.Center;
            float angleTo = Projectile.AngleTo(tar) + MathHelper.PiOver4;
            Projectile.rotation = Utils.AngleLerp(Projectile.rotation, angleTo, 0.15f);
            Projectile.velocity *= 0.92f;
            //切换
            if (DashingTimer > ShootTime)
            {
                DoType = Style.IsFlying;
                DashingTimer = 0;
                Projectile.netUpdate = true;
            }
        }

        private void DoShooted(NPC target)
        {
            if (DashingTimer < SpiningTime / 2)
                DrawSparkLine();
            //缓动
            Projectile.velocity *= 0.95f;
            //转角速度修改
            Vector2 tar = target is null || !target.CanBeChasedBy(Projectile) || !target.active ? Main.MouseWorld : target.Center;
            float angleTo = Projectile.AngleTo(tar) + MathHelper.PiOver4;
            Projectile.rotation = Utils.AngleLerp(Projectile.rotation, angleTo, 0.10f);
            //切换攻击模式
            if (DashingTimer > SpiningTime)
            {
                DoType = Style.IsAngleTo;
                DashingTimer = 0;
                Projectile.netUpdate = true;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (DoType is Style.IsFlying)
            {
                DoType = Style.DropOut;
                Projectile.netUpdate = true;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.GetBaseDrawField(out Texture2D tex, out Vector2 drawPosBase, out Vector2 orig);
            //timeleft剩下12时绘制渐变
            float deadFade = Utils.GetLerpValue(0f, 12f, Projectile.timeLeft, true);
            // rgba(40, 171, 231, 1)
            Color shizukuAqua = Color.White;
            //基础颜色
            Color drawColor = shizukuAqua * Projectile.Opacity * deadFade * 1.5f;
            drawColor.A = 55;
            Color glowColor = shizukuAqua * deadFade;
            glowColor.A = 55;
            //绘制残影与射弹本身
            for (int i = 0; i < 8; i++)
            {
                Vector2 drawPos = drawPosBase - Projectile.velocity * i * 0.45f;
                Color color = drawColor * (1f - i / 8f);
                Main.spriteBatch.Draw(tex, drawPos, null, color, Projectile.rotation, orig, Projectile.scale, SpriteEffects.None, 0.1f);
            }
            return false;
        }

    }
}