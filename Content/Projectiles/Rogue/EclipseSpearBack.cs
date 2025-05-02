using CalamityMod.Buffs.DamageOverTime;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using System;
using CalamityMod;
using CalamityInheritance.Content.Items;
using CalamityMod.Particles;
using CalamityInheritance.Particles;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    // Photoviscerator left click main projectile (the flamethrower itself)
    public class EclipseSpearBack : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = 3;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 5;
            Projectile.timeLeft = 20;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(2))
            {
                Vector2 trailPos = Projectile.Center + Vector2.UnitY.RotatedBy(Projectile.rotation) * Main.rand.NextFloat(-16f, 16f);
                float trailScale = Main.rand.NextFloat(0.8f, 1.2f);
                Color trailColor = Main.rand.NextBool() ? Color.White : Color.DarkOrange;
                Particle eclipseTrail = new SparkParticle(trailPos, Projectile.velocity * 0.2f, false, 60, trailScale, trailColor);
                GeneralParticleHandler.SpawnParticle(eclipseTrail);
            }
            if(Projectile.Calamity().stealthStrike)
            {
                if (Main.rand.NextBool(2))
                {
                    Vector2 trailPos = Projectile.Center + Vector2.UnitY.RotatedBy(Projectile.rotation) * Main.rand.NextFloat(-16f, 16f);
                    float trailScale = Main.rand.NextFloat(0.8f, 1.2f);
                    Color trailColor = Main.rand.NextBool() ? Color.White : Color.DarkOrange;
                    Particle eclipseTrail = new SparkParticle(trailPos, Projectile.velocity * 0.2f, false, 60, trailScale, trailColor);
                    GeneralParticleHandler.SpawnParticle(eclipseTrail);
                }
            }
        }
    }
}
