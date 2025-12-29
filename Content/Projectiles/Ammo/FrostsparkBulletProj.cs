using CalamityMod.Buffs.StatDebuffs;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace CalamityInheritance.Content.Projectiles.Ammo
{
    public class FrostsparkBulletProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 1;
            AIType = ProjectileID.Bullet;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.15f, 0.05f, 0.3f);

            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 4f)
            {
                if (Main.rand.NextBool())
                {
                    int dustType = 15;
                    float spacing = Main.rand.NextFloat(-0.2f, 0.8f);
                    int dust = Dust.NewDust(Projectile.Center - spacing * Projectile.velocity, 1, 1, dustType); ;
                    Main.dust[dust].position = Projectile.Center;
                    Main.dust[dust].velocity *= 0.4f;
                    Main.dust[dust].velocity += Projectile.velocity * 0.7f;
                    Main.dust[dust].noGravity = true;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesFromEdge(Projectile, 0, lightColor);
            return false;
        }

        // This Projectile is always fullbright.
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(1f, 1f, 1f, 0f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damage)
        {
            target.AddBuff(BuffID.OnFire, 240);
            target.AddBuff(BuffID.Frostburn, 240);
            target.AddBuff(BuffType<GlacialState>(), 60);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);

            Projectile.damage /= 3;
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 24;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Damage();

            int num212 = Main.rand.Next(10, 20);
            for (int num213 = 0; num213 < num212; num213++)
            {
                int dustType = Main.rand.Next(2);
                if (dustType == 0)
                {
                    dustType = 67;
                }
                else
                {
                    dustType = 6;
                }
                int num214 = Dust.NewDust(Projectile.Center - Projectile.velocity / 2f, 0, 0, dustType, 0f, 0f, 100, default, 2f);
                Main.dust[num214].velocity *= 2f;
                Main.dust[num214].noGravity = true;
            }
        }
    }
}
