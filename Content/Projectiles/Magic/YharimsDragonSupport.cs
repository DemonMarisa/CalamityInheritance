using System;
using System.Diagnostics.CodeAnalysis;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class YharimsDragonSupport: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        #region 别名
        public ref float AttackType => ref Projectile.ai[0];
        public ref float AttackTimer => ref Projectile.ai[1];
        public ref float TargetIndex => ref Projectile.ai[2];
        public Player Owner => Main.player[Projectile.owner];
        #endregion
        #region 攻击枚举
        const float IsShooted = 0f;
        const float IsBoosting = 1f;
        const float IsKilling = 2f;
        #endregion
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 360; 
            Projectile.width = 56;
            Projectile.height = 64;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }
        public override bool? CanDamage() => AttackType == IsBoosting;
        public override void AI()
        {
            //全程保转角
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            //将NPC提取出来使用，NPC不存在时直接开始试图干掉射弹
            NPC target = Main.npc[(int)TargetIndex];
            if (!target.chaseable || !target.active || target is null)
            {
                AttackType = IsKilling;
                Projectile.netUpdate = true;
            }
            
            switch (AttackType)
            {
                case IsShooted:
                    DoShooted();
                    break;
                case IsBoosting:
                    DoBoosting(target);
                    break;
                case IsKilling:
                    DoKilling();
                    break;
            }
        }

        private void DoKilling()
        {
            if (Projectile.timeLeft > 200)
                Projectile.timeLeft = 200;
            Projectile.timeLeft -= Main.rand.Next(80, 180);
            Projectile.velocity *= 0.98f;
        }
        //执行加速AI.
        private void DoBoosting(NPC target)
        {
    
            float maxSpeed = 18f;
            float acceleration = 0.015f * 15f;
            float homeInSpeed = MathHelper.Clamp(acceleration, 0f, maxSpeed);
            Projectile.HomingNPCBetter(target, 30000f, 12f + homeInSpeed, 0f, 1, null, 0.08f);

            if (Main.rand.NextBool(3))
                TrailDust();
        }

        private void DoShooted()
        {
            if (Projectile.timeLeft < 280)
            {
                AttackType = IsBoosting;
                AttackTimer = 0f;
                Projectile.netUpdate = true;
            }
            else Projectile.velocity *= 0.97f;
        }
        public void TrailDust() 
        {
            for (int i = 0; i < 2 ; i++)
            {
                int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Pixie, 0f, 0f, 0, default, 0.5f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 1f;
                d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Pixie, 0f, 0f, 100, default, 0.5f);
                Main.dust[d].velocity *= 1f;
                Main.dust[d].noGravity = true;
            }
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(CISoundID.SoundFlamethrower, Projectile.position);
            CIFunction.DustCircle(Projectile.position, 8, 0.5f, CIDustID.DustHeatRay, true, 4f);
        }
    }
}