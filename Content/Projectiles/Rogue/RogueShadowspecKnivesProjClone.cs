using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Content.Items.Weapons;
using CalamityMod.Particles;
using System;
using System.IO;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class RogueShadowspecKnivesProjClone: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => GetInstance<RogueShadowspecKnivesProj>().Texture;
        public int DamagePool = 0;
        public int SwitchTime = 0;
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
        #region 攻击枚举
        //攻击打表
        const float IsFlying = 0f;
        const float IsHit = 1f;
        const float IsReturning = 2f;
        #endregion

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.DamageType = GetInstance<RogueDamageClass>();
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            Projectile.DoSyncHandlerWrite(ref writer);
            writer.Write(DamagePool);
            writer.Write(SwitchTime);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.DoSyncHandlerRead(ref reader);
            DamagePool = reader.ReadInt32();
            SwitchTime = reader.ReadInt32();
        }
        public override bool? CanDamage()
        {
            return AttackType == IsFlying;
        }
        public override void AI()
        {
            DoGeneral();
            if (Projectile.ai[0] == -1f)
                AttackType = IsFlying;
            if (SwitchTime > 5)
            {
                AttackType = IsReturning;
                Projectile.netUpdate = true;
            }
            //AI基本上全部重做了
            switch (AttackType)
            {
                case IsFlying:
                    DoFlying();
                    break;
                case IsHit:
                    DoHitting();
                    break;
                case IsReturning:
                    DoReturning();
                    break;
            }
        }

        private void DoReturning()
        {
            CIFunction.BoomerangReturningAI(Owner, Projectile, 20f, 2.4f);
            if (Projectile.Hitbox.Intersects(Owner.Hitbox))
            {
                Owner.Heal(DamagePool / 40);
                Projectile.Kill();
            }
        }

        private void DoGeneral()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
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
        private void DoHitting()
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

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
            {
                int illustrious = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurificationPowder, 0f, 0f, 100, default, 0.8f);
                Main.dust[illustrious].noGravity = true;
                Main.dust[illustrious].velocity *= 1.2f;
                Main.dust[illustrious].velocity -= Projectile.oldVelocity * 0.3f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (AttackType == IsFlying)
            {
                AttackType = IsHit;
                Projectile.netUpdate = true;
                DamagePool += damageDone / 10;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
