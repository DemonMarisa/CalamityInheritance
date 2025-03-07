using CalamityInheritance.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class CosmicFireLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override string Texture => "CalamityInheritance/Content/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 100;
        }

        public override void AI()
        {
            if (Projectile.scale <= 1.5f)
            {
                Projectile.scale *= 1.01f;
            }
            Lighting.AddLight(Projectile.Center, 0.25f, 0f, 0.1f);
            if (Projectile.ai[0] > 7f)
            {
                float scalar = 1f;
                if (Projectile.ai[0] == 8f)
                {
                    scalar = 0.25f;
                }
                else if (Projectile.ai[0] == 9f)
                {
                    scalar = 0.5f;
                }
                else if (Projectile.ai[0] == 10f)
                {
                    scalar = 0.75f;
                }
                Projectile.ai[0] += 1f;
                int dustType = Main.rand.NextBool(4) ? 61 : 62;
                if (Main.rand.NextBool(2))
                {
                    int fire = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 1f);
                    Dust dust = Main.dust[fire];
                    if (Main.rand.NextBool(3))
                    {
                        dust.noGravity = true;
                        dust.scale *= 3f;
                        dust.velocity.X *= 2f;
                        dust.velocity.Y *= 2f;
                    }
                    else
                    {
                        dust.scale *= 1.5f;
                    }
                    dust.velocity.X *= 1.2f;
                    dust.velocity.Y *= 1.2f;
                    dust.scale *= scalar;
                    dust.velocity += Projectile.velocity;
                    if (!dust.noGravity)
                    {
                        dust.velocity *= 0.5f;
                    }
                }
            }
            else
            {
                Projectile.ai[0] += 1f;
            }
            Projectile.rotation += 0.3f * Projectile.direction;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 3;
        }
    }
}
