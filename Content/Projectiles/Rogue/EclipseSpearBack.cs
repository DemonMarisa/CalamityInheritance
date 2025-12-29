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
using LAP.Assets.TextureRegister;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    // Photoviscerator left click main projectile (the flamethrower itself)
    public class EclipseSpearBack : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = GetInstance<RogueDamageClass>();
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
                Particle eclipseTrail = new SparkParticle(trailPos, Projectile.velocity * 0.2f, false, 60, trailScale, Color.DarkOrange);
                Particle eclipseTrai2 = new StarProjBlack(trailPos, Projectile.velocity * 0.2f, false, 60, trailScale, Color.Black);
                GeneralParticleHandler.SpawnParticle(Main.rand.NextBool() ? eclipseTrail : eclipseTrai2);
            }
            if(Projectile.Calamity().stealthStrike)
            {
                if (Main.rand.NextBool(2))
                {
                    Vector2 trailPos = Projectile.Center + Vector2.UnitY.RotatedBy(Projectile.rotation) * Main.rand.NextFloat(-16f, 16f);
                    float trailScale = Main.rand.NextFloat(0.8f, 1.2f);
                    Particle eclipseTrail = new SparkParticle(trailPos, Projectile.velocity * 0.2f, false, 60, trailScale, Color.DarkOrange);
                    Particle eclipseTrai2 = new StarProjBlack(trailPos, Projectile.velocity * 0.2f, false, 60, trailScale, Color.Black);
                    GeneralParticleHandler.SpawnParticle(Main.rand.NextBool() ? eclipseTrail : eclipseTrai2);
                }
            }
        }
    }
}
