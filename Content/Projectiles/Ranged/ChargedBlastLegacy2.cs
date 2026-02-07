using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using CalamityMod;
using CalamityInheritance.Utilities;
using CalamityMod.Particles;
using Microsoft.Xna.Framework.Graphics;
using System;
using LAP.Core.Utilities;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class ChargedBlastLegacy2 : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override string Texture => $"{GenericProjRoute.LaserProjRoute}";
        public Color baseColor = Color.White;
        public int tileHits = 4;
        public Vector2 startVel;
        public bool goToMouse = true;
        bool direction;
        public float homeSpeed;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 5;
            Projectile.timeLeft = 480;
            Projectile.scale = 0.7f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            bool Shredder = Projectile.ai[2] == 0;
            bool Infinity = Projectile.ai[2] == 1;
            bool Svant = Projectile.ai[2] == 2 || Projectile.ai[2] == 3 || Projectile.ai[2] == 4;
            CIFunction.HomeInOnNPC(Projectile, true, 2500f, 18f, 150f);
            if (baseColor == Color.White)
            {
                baseColor = (Infinity ? new Color(229, 49, 39) : Svant ? Color.DarkViolet : Color.DodgerBlue);
                if (Projectile.ai[2] == 3)
                    baseColor = Color.DarkOrchid;
                if (Projectile.ai[2] == 4)
                    baseColor = Color.MediumOrchid;
                startVel = Projectile.velocity;
                if (Infinity)
                {
                    Projectile.timeLeft = 340;
                    Projectile.ArmorPenetration = 25;
                }
                if (Svant)
                {
                    Projectile.timeLeft = 640;
                    Projectile.ArmorPenetration = 100;
                }
                direction = Main.rand.NextBool();
                homeSpeed = Main.rand.NextFloat(5, 8);
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Projectile.timeLeft < 290 && Infinity)
            {
                Projectile.velocity.X = MathHelper.Lerp(Projectile.velocity.X, -startVel.X, 0.02f);
                Projectile.velocity.Y = MathHelper.Lerp(Projectile.velocity.Y, -startVel.Y, 0.02f);
            }
            if (Projectile.timeLeft <= 500 && Svant && goToMouse && Projectile.timeLeft % Projectile.extraUpdates == 0)
            {
                Vector2 mouseSpot = (player.LocalMouseWorld() - Projectile.Center).SafeNormalize(Vector2.UnitX) * homeSpeed;
                Projectile.velocity.X = MathHelper.Lerp(Projectile.velocity.X, mouseSpot.X, 0.085f);
                Projectile.velocity.Y = MathHelper.Lerp(Projectile.velocity.Y, mouseSpot.Y, 0.085f);
                if (Vector2.Distance(Projectile.Center, player.LocalMouseWorld()) < 80)
                {
                    Projectile.timeLeft = 300;
                    Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitX) * homeSpeed / 2;
                    goToMouse = false;
                }
            }
            if (!goToMouse)
            {
                float sine = (float)Math.Sin(Projectile.timeLeft * 0.175f / MathHelper.Pi) * 1.2f;
                Vector2 offset = Projectile.velocity.SafeNormalize(Vector2.UnitX).RotatedBy(MathHelper.PiOver2) * sine * (direction ? 1 : -1);
                Projectile.Center += offset;
            }
            if (Infinity || Svant)
            {
                Projectile.tileCollide = false;
                Projectile.penetrate = -1;
            }
        }
        public override void OnKill(int timeLeft)
        {
            bool Svant = Projectile.ai[2] == 2 || Projectile.ai[2] == 3 || Projectile.ai[2] == 4;
            for (int k = 0; k < (Svant ? 2 : 3); k++)
            {
                Particle spark2 = new LineParticle(Projectile.Center, (Projectile.velocity * 5).RotatedByRandom(0.4f) * Main.rand.NextFloat(0.8f, 1.2f), false, Main.rand.Next(8, 11 + 1), Main.rand.NextFloat(0.5f, 1f), baseColor);
                GeneralParticleHandler.SpawnParticle(spark2);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (baseColor == Color.White)
                return false;
            Texture2D texture = Request<Texture2D>("CalamityMod/Particles/DrainLineBloom").Value;
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], (baseColor * 0.7f) with { A = 0 }, 1, texture);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, baseColor with { A = 0 }, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => CalamityUtils.CircularHitboxCollision(Projectile.Center, 10, targetHitbox);

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            tileHits--;
            if (tileHits <= 0)
            {
                Projectile.Kill();
            }
            else
            {
                if (Projectile.velocity.X != oldVelocity.X)
                    Projectile.velocity.X = -oldVelocity.X;
                if (Projectile.velocity.Y != oldVelocity.Y)
                    Projectile.velocity.Y = -oldVelocity.Y;
            }

            return false;
        }
    }
}
