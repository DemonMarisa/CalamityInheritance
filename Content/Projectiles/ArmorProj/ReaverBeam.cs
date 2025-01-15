using CalamityMod.Particles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Projectiles.ArmorProj
{
    public class ReaverBeam : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Mods.CalamityInheritance.Content.Content.Projectiles";
        public override string Texture => "CalamityInheritance/Content/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.extraUpdates = 70;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.friendly = true;
            Projectile.timeLeft = 500;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (Projectile.timeLeft == 500)
            {
                Particle orb = new CustomPulse(Projectile.Center, Vector2.Zero, Color.YellowGreen, "CalamityMod/Particles/LargeBloom", new Vector2(1, 1), Main.rand.NextFloat(-10, 10), 0.2f, 0.2f, 3);
                GeneralParticleHandler.SpawnParticle(orb);
                Particle orb2 = new CustomPulse(Projectile.Center, Vector2.Zero, Color.ForestGreen, "CalamityMod/Particles/LargeBloom", new Vector2(1, 1), Main.rand.NextFloat(-10, 10), 0.13f, 0.13f, 3);
                GeneralParticleHandler.SpawnParticle(orb2);
            }

            if (Projectile.timeLeft % 3 == 0 && Projectile.timeLeft < 499)
            {
                LineParticle spark2 = new LineParticle(Projectile.Center, -Projectile.velocity * 0.05f, false, 6, 1.8f, Color.Green * 1f);
                GeneralParticleHandler.SpawnParticle(spark2);
                LineParticle spark3 = new LineParticle(Projectile.Center, -Projectile.velocity * 0.05f, false, 6, 1.3f, Color.ForestGreen * 1f);
                GeneralParticleHandler.SpawnParticle(spark3);
            }

        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i <= 4; i++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(4, 4), 107, (Projectile.velocity * 2) * Main.rand.NextFloat(0.1f, 0.9f));
                dust.scale = Main.rand.NextFloat(0.3f, 0.5f);
                dust.noGravity = true;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 90);
            if (hit.Damage > 1)
                Projectile.Kill();
        }
    }
}
