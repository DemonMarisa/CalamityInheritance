using System;
using System.IO;
using System.Security.Cryptography;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Projectiles;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class RogueTypeKnivesEmpyreanProjClone : ModProjectile, ILocalizedModType
    {
        public override string Texture => $"{GenericProjRoute.ProjRoute}/Rogue/RogueTypeKnivesEmpyreanProj";
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        //更逆天的追踪速度与追踪距离，同时三倍其存在时间
        private int bounce = 3;
        #region 攻击类型
        const float IsFlying = 0f;
        const float IsHit = 1f;
        const float IsReturning = 2f;
        #endregion
        #region 基本属性
        public int SwitchTime = 0;
        public int DmaagePool = 0;
        #endregion
        #region 别名
        public ref float AttackType => ref Projectile.ai[0];
        public ref float AttackTimer => ref Projectile.ai[1];
        public int TargetIndex
        {
            get => (int)Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }
        public Player Owner => Main.player[Projectile.owner];
        #endregion
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override bool? CanDamage() => AttackType == IsFlying;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(DmaagePool);
            writer.Write(SwitchTime);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            DmaagePool = reader.ReadInt32();
            SwitchTime = reader.ReadInt32();
        }
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.penetrate = 5;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
            Projectile.extraUpdates = 2;
        }

        public override void AI()
        {
            DoGeneral();
            
            switch (AttackType)
            {
                case IsFlying:
                    DoFlying();
                    break;
                case IsHit:
                    DoHit();
                    break;
                case IsReturning:
                    DoReturning();
                    break;
            }
        }

        public void DoReturning()
        {
            CIFunction.BoomerangReturningAI(Owner, Projectile, 20f, 2.4f);
            if (Projectile.Hitbox.Intersects(Owner.Hitbox))
            {
                int healAmt = Owner.statLifeMax2 / Owner.statLife * (DmaagePool / 40);
                if (healAmt > 20)
                {
                    healAmt = 20 + healAmt / 2;
                }
                Owner.Heal(healAmt);
                Projectile.Kill();
            }
        }

        public void DoHit()
        {
            //Set real NPC instance
            NPC target = Main.npc[TargetIndex];
            //Just in case...
            if (target == null)
                return;
            //现在我们开始hit target。
            AttackTimer += 1f;
            if (AttackTimer >= 25f)
            {
                if (AttackTimer == 25f)
                {
                    Projectile.velocity = Projectile.velocity.RotatedBy(Main.rand.NextBool() ? MathHelper.PiOver4 : -MathHelper.PiOver4);
                    SwitchTime++;
                }
                AttackType = IsFlying;
                Projectile.netUpdate = true;
                AttackTimer = 0f;
            }
        }

        private void DoFlying()
        {
            float dist = (Projectile.Center - Owner.Center).Length();
            if (dist > 3600f)
                Projectile.Kill();
            //寻找距离射弹最近的敌怪，返回这个实例
            NPC target = Projectile.FindClosestTarget(1800f, true);
            //如果target并不为空, 存储NPC
            if (target != null)
            {
                TargetIndex = target.whoAmI;
                //直接追踪这个敌怪。
                Projectile.HomingNPCBetter(target, 1800f, 21f, 20f, 2, default, default, true);
            }
        }

        private void DoGeneral()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (SwitchTime > 3)
            {
                AttackType = IsReturning;
                Projectile.netUpdate = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            bounce--;
            if (bounce <= 0)
                Projectile.Kill();
            else
            {
                if (Projectile.velocity.X != oldVelocity.X)
                    Projectile.velocity.X = -oldVelocity.X;
                if (Projectile.velocity.Y != oldVelocity.Y)
                    Projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
            {
                int empyreanDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Enchanted_Pink, 0f, 0f, 100, default, 0.8f);
                Main.dust[empyreanDust].noGravity = true;
                Main.dust[empyreanDust].velocity *= 1.2f;
                Main.dust[empyreanDust].velocity -= Projectile.oldVelocity * 0.3f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (AttackType == IsFlying)
            {
                AttackType = IsHit;
                Projectile.netUpdate = true;
                DmaagePool += damageDone / 10;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
