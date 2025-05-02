using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Rogue;
using Humanizer;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class EclipseSpearSmall : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
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
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.penetrate = 1;
            Projectile.MaxUpdates = 2;
            Projectile.timeLeft = 75 * Projectile.MaxUpdates;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            CIFunction.HomeInOnNPC(Projectile, true, 600f, 17f, 20f);
        }
        public override void OnHitNPC(NPC npc, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(CISoundMenu.EclipseSpearBoom, npc.Center);
            OnHitSparks();
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<EclipseStealthBoomLegacy>(), Projectile.damage * 2, Projectile.knockBack * Projectile.damage, Projectile.owner);
        }
        public void OnHitSparks()
        {
            int sparkCount = Main.rand.Next(6, 8);
            for (int i = 0; i < sparkCount; i++)
            {
                Vector2 sVel = Projectile.velocity.RotatedByRandom(0.3f) * Main.rand.NextFloat(0.6f, 1.2f);
                int sLife = Main.rand.Next(20, 30);
                float sScale = Main.rand.NextFloat(1.6f, 2f) * 0.955f;
                Color trailColor = Main.rand.NextBool() ? Color.White : Color.DarkOrange;
                Particle spark = new SparkParticle(Projectile.Center, sVel, false, sLife, sScale, trailColor);
                GeneralParticleHandler.SpawnParticle(spark);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 3);
            return false;
        }
    }
}