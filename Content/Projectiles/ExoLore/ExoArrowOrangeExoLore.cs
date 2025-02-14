using CalamityMod.Projectiles;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod;
using CalamityInheritance.Utilities;
using CalamityMod.Particles;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class ExoArrowOrangeExoLore : ModProjectile, ILocalizedModType
    {
        private bool ColorStyle = false;
        private float SizeVariance;
        private float SizeBonus = 2;
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override string Texture => "CalamityInheritance/Content/Projectiles/LaserProj";
        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
            Projectile.Calamity().pointBlankShotDuration = CalamityGlobalProjectile.DefaultPointBlankDuration;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                SizeVariance = Main.rand.NextFloat(0.95f, 1.05f);
                ColorStyle = Main.rand.NextBool();
                Projectile.velocity *= 0.7f;
            }
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 25;
            }
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
            float num55 = 40f;
            float num56 = 1.5f;
            if (Projectile.ai[1] == 0f)
            {
                Projectile.localAI[0] += num56;
                if (Projectile.localAI[0] > num55)
                {
                    Projectile.localAI[0] = num55;
                }
            }
            else
            {
                Projectile.localAI[0] -= num56;
                if (Projectile.localAI[0] <= 0f)
                {
                    Projectile.Kill();
                }
            }
            CalamityUtils.HomeInOnNPC(Projectile, !Projectile.tileCollide, 1000f, 16f, 18f);
        }

        public override Color? GetAlpha(Color lightColor) => new Color(250, 100, 0, Projectile.alpha);

        public override bool PreDraw(ref Color lightColor) => Projectile.DrawBeam(40f, 1.5f, lightColor);

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.ExoDebuffs();
        }
        public override void OnKill(int timeLeft)
        {
            CalamityUtils.ExpandHitboxBy(Projectile, 188);
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 0;
            Projectile.Damage();
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            Vector2 Offset = Main.rand.NextVector2Circular(15, 15);

            Particle explosion = new DetailedExplosion(Projectile.Center + Offset, Vector2.Zero, (ColorStyle ? Color.Orange : Color.OrangeRed) * 0.9f, Vector2.One, Main.rand.NextFloat(-5, 5), 0f, (0.28f * SizeVariance) * SizeBonus, 10);
            GeneralParticleHandler.SpawnParticle(explosion);

            SparkleParticle impactParticle = new SparkleParticle(Projectile.Center + Offset, Vector2.Zero, Color.White, ColorStyle ? Color.Orange : Color.OrangeRed, 2.5f * (SizeBonus * 0.3f), 7, 0f, 2f);
            GeneralParticleHandler.SpawnParticle(impactParticle);

            for (int k = 0; k < 9; k++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, Main.rand.NextBool() ? 262 : 87, new Vector2(4, 4).RotatedByRandom(100) * Main.rand.NextFloat(0.5f, 1.5f));
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(0.7f, 1.25f);
                dust.alpha = 235;
                if (Main.rand.NextBool())
                {
                    Dust dust2 = Dust.NewDustPerfect(Projectile.Center, 303, new Vector2(3, 3).RotatedByRandom(100) * Main.rand.NextFloat(0.5f, 1.5f));
                    dust2.noGravity = true;
                    dust2.scale = Main.rand.NextFloat(0.8f, 1.5f);
                    dust2.alpha = 70;
                }
            }
            for (int k = 0; k < 3; k++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, 278, new Vector2(4, 4).RotatedByRandom(100) * Main.rand.NextFloat(0.5f, 1.5f) + new Vector2(0, -3));
                dust.noGravity = false;
                dust.scale = Main.rand.NextFloat(0.85f, 1f);
                dust.color = ColorStyle ? Color.Orange : Color.OrangeRed;
            }
        }
    }
}
