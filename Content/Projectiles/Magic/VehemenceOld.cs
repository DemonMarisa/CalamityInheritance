using CalamityMod.Projectiles;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;

namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class VehemenceOld : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Lighting.AddLight(Projectile.Center, 0.45f, 0f, 0.45f);
            for (int num457 = 0; num457 < 2; num457++)
            {
                int num458 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 65, 0f, 0f, 100, default, 2f);
                Main.dust[num458].noGravity = true;
                Main.dust[num458].velocity *= 0.15f;
                Main.dust[num458].velocity += Projectile.velocity * 0.1f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item74, Projectile.position);
            for (int j = 0; j <= 25; j++)
            {
                int num459 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 65, 0f, 0f, 100, default, 1f);
                Main.dust[num459].noGravity = true;
                Main.dust[num459].velocity *= 0.1f;
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            double lifeAmount = (double)target.life;
            double lifeMax = (double)target.lifeMax;
            double damageMult = lifeAmount / lifeMax * 7;

            modifiers.SourceDamage.Flat += target.life / 7;

            if (modifiers.SourceDamage.Flat > 1000000f)
            {
                modifiers.SourceDamage.Flat = 1000000f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.life == target.lifeMax)
            {
                target.AddBuff(BuffID.ShadowFlame, 12000);
                target.AddBuff(BuffID.Ichor, 12000);
                target.AddBuff(BuffID.Frostburn, 12000);
                target.AddBuff(BuffID.OnFire, 12000);
                target.AddBuff(BuffID.Poisoned, 12000);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 2);
            return false;
        }
    }
}
