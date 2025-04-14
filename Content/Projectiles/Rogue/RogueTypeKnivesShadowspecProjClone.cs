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

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class RogueTypeKnivesShadowspecProjClone: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => $"{GenericProjRoute.ProjRoute}/Rogue/RogueTypeKnivesShadowspecProj";
        public static readonly float Acceleration = 0.98f; //飞行加速度
        #region 攻击枚举
        //攻击打表
        const float IsHyperFlying = 0f;
        const float IsHitting = 1f;
        const float IsIdling = 2f;
        const float IsReturning = 3f;
        const float IsAngleToPlayer = 4f;
        #endregion
        #region 基础属性
        public int NewTimer = 0;
        const short AllowHitTimer =30;
        public bool alreadyHit = false;
        //转阶段的计时器
        public float PhaseChanger = 0f;
        #endregion
        #region 别名
        const short AttackType = 0;
        const short AttackTimer = 1;
        const short AttackTarget = 2;
        #endregion
        public int HitCounts = 0;
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
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 15;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        public override bool? CanDamage()
        {
            bool canHit = Projectile.ai[0] == IsHitting || Projectile.ai[0] == IsHyperFlying;
            return canHit;
        }
        public override void AI()
        {
            //ref 与别名
            //ref Timer，可能会用得上。谁知道呢
            if (Projectile.ai[0] == -1f)
                Projectile.ai[AttackType] = IsHyperFlying;
            //AI基本上全部重做了
            switch (Projectile.ai[AttackType])
            {
                case IsHyperFlying:
                    DoFlying();
                    break;
                case IsHitting:
                    DoHitting();
                    break;
                case IsIdling:
                    DoIdling();
                    break;
                case IsAngleToPlayer:
                    DoAngleToPlayer();
                    break;
                case IsReturning:
                    DoReturning();
                    break;
            }
            if (HitCounts > 5) Projectile.ai[AttackType] = IsReturning;
        }

        private void DoAngleToPlayer()
        {
            Player plr = Main.player[Projectile.owner];
            //实例如果存在, just in case
            float rotAngle = Projectile.AngleTo(plr.Center) + MathHelper.PiOver2;
            Projectile.rotation += Utils.AngleLerp(Projectile.rotation, rotAngle, 0.2f);
            //Raise the Timer
            Projectile.ai[AttackTimer] += 1f;
            //Timer is up, do hitting
            if (Projectile.ai[AttackTimer] > AllowHitTimer) 
            {
                Projectile.ai[AttackType] = IsReturning;
                //Reset Tiemr
                Projectile.ai[AttackTimer] = 0f;
            }
            Main.NewText("IsAngleToPlayer");
        }
        private void DoReturning()
        {
            //直接照抄回旋镖的AI。
            Projectile.tileCollide = false;
            float rSpeed = 16f;
            float accele = 3.2f;
            Player plr = Main.player[Projectile.owner];
            CIFunction.BoomerangReturningAI(plr, Projectile, rSpeed, accele);
            if (Main.myPlayer == Projectile.owner)
            {
                Rectangle rectangle = new((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                Rectangle value2 = new((int)Main.player[Projectile.owner].position.X, (int)Main.player[Projectile.owner].position.Y, Main.player[Projectile.owner].width, Main.player[Projectile.owner].height);
                if (rectangle.Intersects(value2))
                {
                    //在接触玩家的时候我们强制其发起治疗
                    int healAmt = plr.statLifeMax2 / plr.statLife;
                    plr.Heal(healAmt);
                    Projectile.Kill();
                    Projectile.netUpdate = true;
                }
            }
            Main.NewText("IsReturning");
        }

        private void DoFlying()
        {
            Projectile.rotation += Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Player p = Main.player[Projectile.owner];
            //超高速飞行，这个飞行一般情况下只会执行一次。
            //光效，从隔壁孔雀翎拿过来的
            SparkParticle line = new SparkParticle(Projectile.Center - Projectile.velocity * 1.1f, Projectile.velocity * 0.01f, false, 18, 1f, Color.SeaGreen);
            GeneralParticleHandler.SpawnParticle(line);
            //不断检索其与玩家的距离，如果超出了玩家一定距离，杀死这个射弹
            float dist = (Projectile.Center - p.Center).Length();
            if (dist > 3600f)
                Projectile.Kill();
            //直接追踪敌怪
            CIFunction.HomeInOnNPC(Projectile, true, 3600f, 20f, 20f, 20f);
        }
        private void DoHitting()
        {
            //Set real NPC instance
            NPC target = Main.npc[(int)Projectile.ai[AttackTarget]];
            //Just in case...
            if (target== null)
                return;
            //现在我们开始hit target。
            Projectile.extraUpdates = 1;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            CIFunction.HomeInOnNPC(Projectile, true, 1800f, 20f, 20f);
            Projectile.ai[AttackTimer] = 0f;
        }
        private void DoIdling()
        {
            //转实际NPC实例
            NPC realTarget = Main.npc[(int)Projectile.ai[AttackTarget]];
            //实例如果存在, just in case
            if (realTarget != null)
            {
                //Timer is up, do hitting
                if (Projectile.ai[AttackTimer] > AllowHitTimer) 
                {
                    Projectile.ai[AttackType] = IsHitting;
                    //Reset Tiemr
                    Projectile.ai[AttackTimer] = 0f;
                }
                else
                {
                    if (Projectile.ai[AttackTimer] == 2f)
                        Main.NewText("IsDoingHit");
                    float rotAngle = Projectile.AngleTo(realTarget.Center) + MathHelper.PiOver2;
                    Projectile.rotation = Utils.AngleLerp(Projectile.rotation, rotAngle, 0.2f);
                    //Raise the Timer
                    Projectile.ai[AttackTimer] += 1f;
                }
            }
            else Main.NewText("NPC NOT FOUND");
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
            if (!alreadyHit && Projectile.ai[0] == IsHyperFlying)
            {
                //首次击中敌人，使其进入挂载模式
                Projectile.ai[AttackType] = IsIdling;
                //重设定eu
                Projectile.extraUpdates = 1;
                //将敌怪npc单位存储进去
                Projectile.ai[AttackTarget] = target.whoAmI;
                //?
                Main.NewText("IsAlreadyHit");
                //将其设置为真，且永远为真。
                alreadyHit = true;
            }
            if (Projectile.ai[AttackType] == IsHitting)
            {
                //开始发起追踪攻击的时候，我们使其切换为挂载模式
                Projectile.ai[AttackType] = IsIdling;
                //just in case...
                Projectile.ai[AttackTarget] = target.whoAmI;
                Main.NewText("IsHitting");
                //每次这个追踪攻击击中一次敌人，+1.我们最多使其击中5次。
                HitCounts++;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
