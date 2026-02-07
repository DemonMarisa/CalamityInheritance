using System.IO;
using CalamityInheritance.Buffs.Statbuffs;
using CalamityInheritance.Particles;
using CalamityInheritance.Sounds.Custom.Shizuku;
using CalamityInheritance.Utilities;
using CalamityMod.Particles;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
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
        public Vector2 _lastMousePosition = Vector2.One;
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
        public ref float InitAngle => ref Projectile.CalamityInheritance().ProjNewAI[1];
        public bool NotStirke = true;
        private const float SpiningTime = 45;
        private const float ShootTime = 25; 
        public override void SetStaticDefaults() => ProjectileID.Sets.CultistIsResistantTo[Type] = true;
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
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
            if (!target.CanBeChasedBy(Projectile))
                target = LAPUtilities.FindClosestTarget(Projectile.Center, 1800f);
            DashingTimer += 1;
            Projectile.Opacity = Utils.GetLerpValue(0f, 1f, DashingTimer, true) * Utils.GetLerpValue(0f, 1f, Projectile.timeLeft, true);
            //缓动，减速，与自转，类似于隔壁ACT射弹
            switch (DoType)
            {
                case Style.IsShooted:
                    DoShooted();
                    break;
                case Style.IsAngleTo:
                    DoAngleTo(target);
                    break;
                case Style.IsFlying:
                    DoFlying(target);
                    break;
                case Style.DropOut:
                    Droppper(target);
                    break;
            }
            //计算玩家后方基准位置（基于鼠标方向）
            float rearDistance = 120f;
            Vector2 targetRearPosition = Owner.Center + InitAngle.ToRotationVector2() * rearDistance;
            bool isRearPosState = DoType is Style.IsShooted || DoType is Style.IsAngleTo;
            if (isRearPosState)
            {
                float lerpFac = DoType is Style.IsShooted ? 0.12f : 0.18f;
                Projectile.Center = Vector2.Lerp(Projectile.Center, targetRearPosition, lerpFac);
            }
        }

        private void Droppper(NPC target)
        {
            if (!target.LegalTarget(Projectile))
                Projectile.Kill();

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            Projectile.HomingNPCBetter(target, 20f + AttackTimer/3, 20f, 1);
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
            bool shouldPointToMouse = !target.LegalTarget(Projectile);
            //如果符合，设定射弹初始为指向指针的速度。且不再变化
            DrawSparkLine();
            if (shouldPointToMouse)
            {
                if (Projectile.timeLeft > 50)
                    Projectile.timeLeft = 50;
                if (AttackTimer is 0)
                {
                    Player player = Main.player[Projectile.owner];
                    Vector2 direction = (Projectile.Center - player.LocalMouseWorld()).SafeNormalize(Vector2.UnitX);
                    Projectile.velocity = direction * 23;
                    Projectile.rotation = Projectile.velocity.ToRotation() +  MathHelper.PiOver4;
                    AttackTimer = -1;
                }
            }
            //敌对单位如果可用，则正常冲向敌对单位
            else if (AttackTimer != -1 && !shouldPointToMouse)
            {
                float angleTo = Projectile.AngleTo(target.Center) + MathHelper.PiOver4;
                if (AttackTimer is 0)
                {
                    Projectile.velocity = (angleTo - MathHelper.PiOver4).ToRotationVector2() * 24f;
                }
                AttackTimer += 1;
                if (AttackTimer > 15)
                {
                    Projectile.rotation = MathHelper.Lerp(Projectile.rotation, Projectile.velocity.ToRotation() + MathHelper.PiOver4, 0.2f);
                    if (NotStirke)
                        Projectile.HomingNPCBetter(target, 20f + AttackTimer, 20f, 1);
                }
                else
                    Projectile.rotation = Utils.AngleLerp(Projectile.rotation, angleTo, 0.15f);

                if (AttackTimer > 60f)
                {
                    DoType = Style.DropOut;
                    Projectile.netUpdate = true;
                }
            }
        }
        private void DoAngleTo(NPC target)
        {
            //指向你的敌人。
            Vector2 tar = target.LegalTarget(Projectile) ? target.Center : Owner.Center;
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
        private void DoShooted()
        {
            if (DashingTimer < SpiningTime / 2)
                DrawSparkLine();
            //缓动
            Projectile.velocity *= 0.95f;
            //转角速度修改
            Vector2 tar = Owner.Center;
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
                Projectile.netUpdate = true;
                //非返程追踪时每次命中获得10%乘算增伤
                Projectile.damage = (int)(Projectile.damage * 1.10);
                SoundStyle strikeSound = Utils.SelectRandom(Main.rand, ShizukuSounds.DaggerHitDirectly.ToArray());
                SoundEngine.PlaySound(strikeSound, target.Center);
                NotStirke = false;
            }
            if (DoType is Style.DropOut)
            {
                SoundEngine.PlaySound(ShizukuSounds.DaggerHit with { MaxInstances = 1 }, target.Center);
                //击杀射弹
                Projectile.Kill();
            }
            target.AddBuff(BuffType<ShizukuMoonlight>(), 600);
            Owner.CIMod().moonClass = ShizukuMoonlight.ClassType.Magic;
            target.CIMod().moonClass = ShizukuMoonlight.ClassType.Magic;
            Owner.AddBuff(BuffType<ShizukuMoonlight>(), 60);
            
            
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
                Main.spriteBatch.Draw(tex, drawPos, null, color with { A = 50 }, Projectile.rotation, orig, Projectile.scale, SpriteEffects.None, 0.1f);
            }
            return false;
        }

    }
}