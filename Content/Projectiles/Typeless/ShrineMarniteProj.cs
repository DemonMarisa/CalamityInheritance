using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Projectiles.Typeless
{
    public class ShrineMarniteProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Contente.Projectiles.Typeless";
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void AI()
        {
            if (Projectile.velocity.X != Projectile.velocity.X)
            {
                Projectile.velocity.X = Projectile.velocity.X * -0.1f;
            }
            if (Projectile.velocity.X != Projectile.velocity.X)
            {
                Projectile.velocity.X = Projectile.velocity.X * -0.5f;
            }
            if (Projectile.velocity.Y != Projectile.velocity.Y && Projectile.velocity.Y > 1f)
            {
                Projectile.velocity.Y = Projectile.velocity.Y * -0.5f;
            }
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 5f)
            {
                Projectile.ai[0] = 5f;
                if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
                {
                    Projectile.velocity.X = Projectile.velocity.X * 0.97f;
                    if (Projectile.velocity.X > -0.01 && Projectile.velocity.X < 0.01)
                    {
                        Projectile.velocity.X = 0f;
                        Projectile.netUpdate = true;
                    }
                }
                Projectile.velocity.Y = Projectile.velocity.Y + 0.2f;
            }
            Projectile.rotation += Projectile.velocity.X * 0.1f;
            int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.UnusedWhiteBluePurple, 0f, 0f, 100, default, 0.5f);
            Dust dClone = Main.dust[d];
            dClone.position.X -= 2f;
            Dust dClone2 = Main.dust[d];
            dClone2.position.Y += 2f;
            Main.dust[d].scale += Main.rand.Next(50) * 0.01f;
            Main.dust[d].noGravity = true;
            Dust dClone3 = Main.dust[d];
            dClone3.velocity.Y -= 2f;
            if (Main.rand.NextBool(2))
            {
                int d2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.UnusedWhiteBluePurple, 0f, 0f, 100, default, 0.5f);
                Dust d2Clone = Main.dust[d2];
                d2Clone.position.X -= 2f;
                Dust d2Clone2 = Main.dust[d2];
                d2Clone2.position.Y += 2f;
                Main.dust[d2].scale += 0.3f + Main.rand.Next(50) * 0.01f;
                Main.dust[d2].noGravity = true;
                Main.dust[d2].velocity *= 0.1f;
            }
            if (Projectile.velocity.Y < 0.25 && Projectile.velocity.Y > 0.15)
            {
                Projectile.velocity.X = Projectile.velocity.X * 0.8f;
            }
            Projectile.rotation = -Projectile.velocity.X * 0.05f;
            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.penetrate == 0)
            {
                Projectile.Kill();
            }
            return false;
        }
    }
}