using System;
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
        public static readonly int GodSlayerKnivesLifeStealCap = 500;
        public static readonly int GodSlayerKnivesLifeTime = 900;
        public static readonly float GodSlayerKnivesLifeStealRange = 3000f;
        public static readonly float GodSlayerKnivesChasingSpeed = 9f;
        public static readonly float GodSlayerKnivesChasingRange = 1500f;
        //更逆天的追踪速度与追踪距离，同时三倍其存在时间
        private int bounce = 3;
        #region 攻击类型
        const int AType = 0;
        const int Timer = 1;
        const int Target = 2;
        const float DoFlying = 0f;
        const float DoAttacking = 1f;
        const float DoReturning = 2f;
        #endregion
        #region 基本属性
        public int RotatingTime = 0;
        public int HitTime = 0;
        const float BeginHomingTimer = 30f;
        const float HomingSpeed = 28f;
        const float HomingInerit = 5f;
        #endregion

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override bool? CanDamage()
        {
            return Projectile.ai[AType] == DoAttacking || (Projectile.ai[AType] == DoFlying);
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
            if (Projectile.penetrate <=1)
            {
                Projectile.ai[AType] = DoReturning;
            }
            switch (Projectile.ai[AType])    
            {
                case DoFlying:
                    DoFlyingAI();
                    break;
                case DoAttacking:
                    DoAttackingAI();
                    break;
                case DoReturning:
                    DoReturningAI();
                    break;
            }
        }

        private void DoReturningAI()
        {
            //直接照抄回旋镖的AI。
            Projectile.tileCollide = false;
            float rSpeed = 16f;
            float accele = 3.2f;
            Player plr = Main.player[Projectile.owner];
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
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
        }

        private void DoAttackingAI()
        {
            if (Projectile.ai[Timer] > BeginHomingTimer / 10f)
            {

                if (Projectile.ai[Timer] == BeginHomingTimer)
                    Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[Timer]);
                CIFunction.HomeInOnNPC(Projectile, false, 1800f, HomingSpeed, HomingInerit);
                Projectile.ai[Timer] = 0f;
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                Projectile.ai[Timer] += 0.1f;
            }
        }

        private void DoFlyingAI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.ai[Timer] += 1f;
            if (Projectile.ai[Timer] > BeginHomingTimer / 3)
            {
                CIFunction.HomeInOnNPC(Projectile, false, 1800f, HomingSpeed, HomingInerit);
                Projectile.ai[Timer] = 0f;
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
            if (Projectile.ai[AType] == DoFlying)
                Projectile.ai[AType] = DoAttacking;
            
            if (Projectile.ai[AType] == DoAttacking)
            {
                HitTime += 1;
                Projectile.ai[AType] = DoFlying;
            }


            int heal = (int)Math.Round(hit.Damage * 0.015);
            if (heal > GodSlayerKnivesLifeStealCap)
                heal = GodSlayerKnivesLifeStealCap;

            if (Main.player[Main.myPlayer].lifeSteal <= 0f || heal <= 0 || target.lifeMax <= 5)
                return;

            CalamityGlobalProjectile.SpawnLifeStealProjectile(Projectile, Main.player[Projectile.owner], heal, ProjectileID.VampireHeal, GodSlayerKnivesLifeStealRange);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            int heal = (int)Math.Round(info.Damage * 0.015);
            if (heal > GodSlayerKnivesLifeStealCap)
                heal = GodSlayerKnivesLifeStealCap;

            if (Main.player[Main.myPlayer].lifeSteal <= 0f || heal <= 0)
                return;

            CalamityGlobalProjectile.SpawnLifeStealProjectile(Projectile, Main.player[Projectile.owner], heal, ProjectileID.VampireHeal, GodSlayerKnivesLifeStealRange);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
