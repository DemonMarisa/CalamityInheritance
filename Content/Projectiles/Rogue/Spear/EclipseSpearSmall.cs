using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using LAP.Content.Particles;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Rogue.Spear
{
    public class EclipseSpearSmall : CIRogueProj
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = RogueDamage.Instance;
            Projectile.penetrate = 1;
            Projectile.MaxUpdates = 3;
            Projectile.timeLeft = 75 * Projectile.MaxUpdates;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            LAPUtilities.HomeInNPC(Projectile, 600f, 17f, 20f);
        }
        public override void OnHitNPC(NPC npc, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(CISounds.EclipseSpearBoom, npc.Center);
            OnHitSparks();
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ProjectileType<EclipseStealthBoomLegacy>(), Projectile.damage * 2, Projectile.knockBack * Projectile.damage, Projectile.owner);
        }
        public void OnHitSparks()
        {
            int sparkCount = Main.rand.Next(6, 8);
            for (int i = 0; i < sparkCount; i++)
            {
                Vector2 sVel = Projectile.velocity.RotatedByRandom(0.3f) * Main.rand.NextFloat(0.6f, 1.2f);
                int sLife = Main.rand.Next(20, 30);
                float sScale = Main.rand.NextFloat(1.6f, 2f) * 0.955f;
                Color trailColor = Color.DarkOrange;
                if (Main.rand.NextBool())
                {
                    SparkParticle eclipseTrail = new SparkParticle(Projectile.Center, sVel, false, sLife, sScale, trailColor);
                    eclipseTrail.Spawn();
                }
                else
                {
                    SparkParticle eclipseTrai2 = new SparkParticle(Projectile.Center, sVel, false, sLife, sScale, Color.Black);
                    eclipseTrai2.SpawnToNonPreMult();
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 3);
            return false;
        }
    }
}