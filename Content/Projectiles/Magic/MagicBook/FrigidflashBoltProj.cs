using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Items.Weapons.Magic.MagicBook;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic.MagicBook
{
    public class FrigidflashBoltProj : CIMagicProj
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.alpha = 255;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 480;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 25;
                if (Projectile.alpha < 0)
                    Projectile.alpha = 0;
            }
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.25f / 255f, (255 - Projectile.alpha) * 0.25f / 255f, (255 - Projectile.alpha) * 0.25f / 255f);
            for (int i = 0; i < 2; i++)
            {
                float shortXVel = Projectile.velocity.X / 3f * i;
                float shortYVel = Projectile.velocity.Y / 3f * i;
                int fourConst = 4;
                int fireDust = Dust.NewDust(new Vector2(Projectile.position.X + fourConst, Projectile.position.Y + fourConst), Projectile.width - fourConst * 2, Projectile.height - fourConst * 2, DustID.InfernoFork, 0f, 0f, 100, default, 1.2f);
                Dust dust = Main.dust[fireDust];
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += Projectile.velocity * 0.1f;
                dust.position.X -= shortXVel;
                dust.position.Y -= shortYVel;
            }
            if (Main.rand.NextBool(10))
            {
                int otherFourConst = 4;
                int fireDustSmol = Dust.NewDust(new Vector2(Projectile.position.X + otherFourConst, Projectile.position.Y + otherFourConst), Projectile.width - otherFourConst * 2, Projectile.height - otherFourConst * 2, DustID.InfernoFork, 0f, 0f, 100, default, 0.6f);
                Main.dust[fireDustSmol].velocity *= 0.25f;
                Main.dust[fireDustSmol].velocity += Projectile.velocity * 0.5f;
            }
            for (int j = 0; j < 2; j++)
            {
                float shortyX = Projectile.velocity.X / 3f * j;
                float shortyY = Projectile.velocity.Y / 3f * j;
                int itsAFour = 4;
                int frostDust = Dust.NewDust(new Vector2(Projectile.position.X + itsAFour, Projectile.position.Y + itsAFour), Projectile.width - itsAFour * 2, Projectile.height - itsAFour * 2, DustID.Frost, 0f, 0f, 100, default, 1.2f);
                Dust dust = Main.dust[frostDust];
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += Projectile.velocity * 0.1f;
                dust.position.X -= shortyX;
                dust.position.Y -= shortyY;
            }
            if (Main.rand.NextBool(10))
            {
                int itsStillAFour = 4;
                int frostDustSmol = Dust.NewDust(new Vector2(Projectile.position.X + itsStillAFour, Projectile.position.Y + itsStillAFour), Projectile.width - itsStillAFour * 2, Projectile.height - itsStillAFour * 2, DustID.Frost, 0f, 0f, 100, default, 0.6f);
                Main.dust[frostDustSmol].velocity *= 0.25f;
                Main.dust[frostDustSmol].velocity += Projectile.velocity * 0.5f;
            }
            Projectile.rotation += 0.3f * Projectile.direction;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
            {
                Projectile.Kill();
            }
            else
            {
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }
                SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
            }
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(FrigidflashBoltLegacy.ProjDeathSound, Projectile.Center);
            for (int k = 0; k < 3; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.InfernoFork, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Frost, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 180);
            target.AddBuff(BuffID.Frostburn, 180);
        }
    }
}