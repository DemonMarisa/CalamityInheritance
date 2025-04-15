using System;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class DukeLegendaryRazorClone: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override string Texture => "CalamityMod/Projectiles/TornadoProj";
        public bool SkyFall = false; 
        #region 攻击顺序枚举
        const float IsShooted = 0f;
        const float IsHoming = 1f;
        #endregion
        #region 数组别名
        const short AttackType = 0;
        const short AttackTimer = 1;
        #endregion
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 2;
            Projectile.tileCollide = false;
            Projectile.penetrate = 2;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
        }
        public override bool? CanDamage()
        {
            return Projectile.ai[AttackType] == IsHoming;
        }
        public override void AI()
        {
            Projectile.localAI[1] += 1f;
            if (Projectile.localAI[1] > 4f && Main.rand.NextBool(4))
            {
                for (int i = 0; i < 3; i++)
                {
                    int blueDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Flare_Blue, 0f, 0f, 100, new Color(53, Main.DiscoG, 255), 2f);
                    Main.dust[blueDust].noGravity = true;
                    Main.dust[blueDust].velocity *= 0f;
                }
     
            }
            Projectile.alpha -= 100;
            if (Projectile.alpha < 5)
            {
                Projectile.alpha = 5;
            }
            //重做AI
            Projectile.rotation += Projectile.velocity.X * 0.7f;
            //如往常一样，搜索距离最近的敌怪
            NPC target = CIFunction.FindClosestTarget(Projectile, 2400f, true, true);
            //如果这玩意为空，直接返回，即不执行下方尝试减速等的AI
            if (target == null)
            {
                Projectile.netUpdate = true;
                return;
            }
            switch (Projectile.ai[AttackType])
            {
                //首先我们会让其飞行一段时间
                case IsShooted:
                    DoShooted();
                    break;
                case IsHoming:
                    DoHoming(target);
                    break;
            }
        }

        private void DoHoming(NPC target)
        {
            //发起追踪, 有一个逐渐加速的过程
            if (Projectile.ai[AttackTimer] < 25f)
                Projectile.ai[AttackTimer] += 1f;
            CIFunction.HomingNPCBetter(Projectile, target, 2400f, 10f + Projectile.ai[AttackTimer], 20f, 2, 14f);
            //只有发起追踪的才会有轨迹
            if(Main.rand.NextBool(4))
                TrailLine();
        }

        public void DoShooted()
        {
            //使其飞行一段时间同时一直减速
            Projectile.ai[AttackTimer] += 1f;
            if (Projectile.ai[AttackTimer] > 30f)
            Projectile.velocity *= 0.90f;
            if (Projectile.ai[AttackTimer] > 60f) 
            {
                Projectile.ai[AttackType] = IsHoming;
                Projectile.ai[AttackTimer] = 0f;
                Projectile.netUpdate = true;
            }
        }

        public void TrailLine()
        {
            if (Main.rand.NextBool(2))
            {
                Vector2 trailPos = Projectile.Center + Vector2.UnitY.RotatedBy(Projectile.rotation) * Main.rand.NextFloat(-16f, 16f);
                float trailScale = Main.rand.NextFloat(0.8f, 1.2f);
                Color trailColor = Main.rand.NextBool() ? Color.Aquamarine : Color.SkyBlue;
                Particle trail = new SparkParticle(trailPos, Projectile.velocity * 0.2f, false, 60, trailScale, trailColor);
                GeneralParticleHandler.SpawnParticle(trail);
            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 10; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Flare_Blue, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 100, new Color(53, Main.DiscoG, 255));
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(53, 236, 255, Projectile.alpha);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
