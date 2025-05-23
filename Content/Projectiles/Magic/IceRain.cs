using System;
using System.Diagnostics;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic
{

    public class IceRain: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic" ;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public ref float AttackType => ref Projectile.ai[0];
        public ref float AttackTimer => ref Projectile.ai[1];
        const float IsShooted = 0f;
        const float IsHoming = 1f;
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.scale *= 1f;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 240;
        }

        public override void AI()
        {
            DoGeneric();
            // AttackTimer++;
            // switch(AttackType)
            // {
            //     case IsShooted:
            //         DoShooted();
            //         break;
            //     case IsHoming:
            //         DoHoming();
            //         break;
            // }
            Projectile.rotation += 0.5f;
            Projectile.ai[0] += 1f;
            // CreateDust();
            if(Projectile.ai[0] > 30f) //在原代码，这个弹幕是试图去找敌人的而不是什么别的，这里则直接改成了，直接追踪
            {
                if(Projectile.ai[0] < 40f)
                    Projectile.velocity *=0.5f;
                if(Projectile.ai[0] == 42f)
                    SignalSend();
                if(Projectile.ai[0] > 45f)
                {
                    Projectile.rotation += Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                    CIFunction.HomeInOnNPC(Projectile, true, 1800f, 12f, 10f);
                }
            }   
        }

        private void DoGeneric() => CreateDust();

        private void DoHoming()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            CIFunction.HomeInOnNPC(Projectile, true, 1800f, 12f, 10f);
        }

        private void DoShooted()
        {
            AttackTimer++;
            if (AttackTimer < 30f)
                return;
            if (AttackTimer < 45f)
                Projectile.velocity *= 0.5f;
            else
            {
                if (AttackTimer == 45f)
                    SignalSend();
                AttackType = IsHoming;
                Projectile.netUpdate = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item30, Projectile.Center);
            for(int i = 0; i < 5; i ++)
                Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.IceRod, Projectile.velocity.X, Projectile.velocity.Y);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn2, 360);
            target.AddBuff(BuffID.Frostburn, 360);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Frostburn2, 360);
            target.AddBuff(BuffID.Frostburn, 360);
        }
        public void CreateDust()
        {
            int newDust = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.IceRod, 0f, 0f, 100, default, 0.7f);
            Main.dust[newDust].noGravity = true;
            Main.dust[newDust].velocity *= 0f;
        }
        public void SignalSend()
        {
            SoundEngine.PlaySound(SoundID.Item30, Projectile.Center);
            CIFunction.DustCircle(Projectile.Center, 16f, 1.2f, DustID.SnowflakeIce, true, 8f);
        }
    }
}