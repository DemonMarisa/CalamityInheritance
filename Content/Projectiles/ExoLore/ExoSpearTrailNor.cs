using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.ExoLore
{

    public class ExoSpearTrailNor : ModProjectile
    {
        private int bounces = 4;

        private bool firstTick = true;

        private float projectileAcceleration = 8f;

        private float topSpeed = 24f;

        private int timer;

        private NPC target;

        private NPC possibleTarget;

        private bool foundTarget;

        private float maxDistance = 1500f;

        private float distance;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.arrow = false;
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.timeLeft = 180;
        }

        public override void AI()
        {
            Projectile.CalamityInheritance().PingReducedNanoFlare = true;
            if (firstTick)
            {
                Projectile.frame = Main.rand.Next(2);
                firstTick = false;
            }
            _ = Main.player[Projectile.owner];
            timer++;
            if (timer > 20)
            {
                for (int i = 0; i < 200; i++)
                {
                    possibleTarget = Main.npc[i];
                    distance = (possibleTarget.Center - Projectile.Center).Length();
                    if (distance < maxDistance && possibleTarget.active && !possibleTarget.dontTakeDamage && possibleTarget.chaseable && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal && Collision.CanHit(Projectile.Center, 0, 0, possibleTarget.Center, 0, 0))
                    {
                        target = Main.npc[i];
                        foundTarget = true;
                        maxDistance = (target.Center - Projectile.Center).Length();
                    }
                }
                if (foundTarget)
                {
                    Vector2 value = target.Center - Projectile.Center;
                    value.Normalize();
                    value.X *= projectileAcceleration;
                    value.Y *= projectileAcceleration;
                    Projectile projectile = Projectile;
                    projectile.velocity = projectile.velocity + value;
                    if (Projectile.velocity.Length() > topSpeed)
                    {
                        Projectile.velocity = Utils.SafeNormalize(Projectile.velocity, -Vector2.UnitY) * topSpeed;
                    }
                    if (!target.active)
                    {
                        foundTarget = false;
                    }
                }
            }
            maxDistance = 800f;
            Projectile.rotation = Utils.ToRotation(Projectile.velocity) + MathHelper.ToRadians(45f);
            Vector2 value2 = new Vector2(0f, 0f);
            Dust.NewDustPerfect(Projectile.Center, 247, (Vector2?)value2, 0, default, 1f);
            Lighting.AddLight(Projectile.Center, 0f, 0.7f, 1f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 0;
            Projectile.Kill();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (bounces <= 0)
            {
                Projectile.Kill();
            }
            else
            {
                Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
                SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = 0f - oldVelocity.X;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = 0f - oldVelocity.Y;
                }
            }
            bounces--;
            return false;
        }
    }
}
