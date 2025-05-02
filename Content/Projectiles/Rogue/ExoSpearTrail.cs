using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class ExoSpearTrail : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        private int bounces = 4;

        private bool firstTick = true;

        private float projectileAcceleration = 8f;

        private float topSpeed = 24f;

        private int timer;

        private NPC target;

        private NPC possibleTarget;

        private bool foundTarget;

        private float maxDistance = 900f;

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
            maxDistance = 1600f;
            Projectile.rotation = Utils.ToRotation(Projectile.velocity) + MathHelper.ToRadians(45f);
            int num309 = Dust.NewDust(new Vector2(Projectile.position.X - Projectile.velocity.X * 4f + 2f, Projectile.position.Y + 2f - Projectile.velocity.Y * 4f), 8, 8, DustID.TerraBlade, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(0, 255, 255), 0.5f);
            Main.dust[num309].velocity *= -0.25f;
            num309 = Dust.NewDust(new Vector2(Projectile.position.X - Projectile.velocity.X * 4f + 2f, Projectile.position.Y + 2f - Projectile.velocity.Y * 4f), 8, 8, DustID.TerraBlade, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(0, 255, 255), 0.5f);
            Main.dust[num309].velocity *= -0.25f;
            Main.dust[num309].position -= Projectile.velocity * 0.5f;

            Dust.NewDustPerfect(Projectile.Center, 247, (Vector2?)new Vector2(0f, 0f), 0, default(Color), 1f);

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
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Zombie103, Projectile.position);
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 80;
            Projectile.position.X = Projectile.position.X - Projectile.width / 2;
            Projectile.position.Y = Projectile.position.Y - Projectile.height / 2;
            for (int num193 = 0; num193 < 2; num193++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f, 100, new Color(0, 255, 255), 1.5f);
            }
            for (int num194 = 0; num194 < 20; num194++)
            {
                int num195 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f, 0, new Color(0, 255, 255), 2.5f);
                Main.dust[num195].noGravity = true;
                Main.dust[num195].velocity *= 3f;
                num195 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f, 100, new Color(0, 255, 255), 1.5f);
                Main.dust[num195].velocity *= 2f;
                Main.dust[num195].noGravity = true;
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.timeLeft >= 160)
            {
                return false;
            }
            return null;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 3);
            return false;
        }
    }
}
