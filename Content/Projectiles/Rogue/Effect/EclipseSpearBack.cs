using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Core.Utils;
using LAP.Assets.TextureRegister;
using LAP.Content.Particles;
using Microsoft.Xna.Framework;
using Terraria;

namespace CalamityInheritance.Content.Projectiles.Rogue.Effect
{
    public class EclipseSpearBack : CIRogueProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = RogueDamage.Instance;
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
                if (Main.rand.NextBool())
                {
                    SparkParticle eclipseTrail = new SparkParticle(trailPos, Projectile.velocity * 0.2f, false, 60, trailScale, Color.DarkOrange);
                    eclipseTrail.Spawn();
                }
                else
                {
                    SparkParticle eclipseTrai2 = new SparkParticle(trailPos, Projectile.velocity * 0.2f, false, 60, trailScale, Color.Black);
                    eclipseTrai2.SpawnToNonPreMult();
                }
            }
            if (Projectile.CI().Stealth)
            {
                if (Main.rand.NextBool(2))
                {
                    Vector2 trailPos = Projectile.Center + Vector2.UnitY.RotatedBy(Projectile.rotation) * Main.rand.NextFloat(-16f, 16f);
                    float trailScale = Main.rand.NextFloat(0.8f, 1.2f);
                    if (Main.rand.NextBool())
                    {
                        SparkParticle eclipseTrail = new SparkParticle(trailPos, Projectile.velocity * 0.2f, false, 60, trailScale, Color.DarkOrange);
                        eclipseTrail.Spawn();
                    }
                    else
                    {
                        SparkParticle eclipseTrai2 = new SparkParticle(trailPos, Projectile.velocity * 0.2f, false, 60, trailScale, Color.Black);
                        eclipseTrai2.SpawnToNonPreMult();
                    }
                }
            }
        }
    }
}
