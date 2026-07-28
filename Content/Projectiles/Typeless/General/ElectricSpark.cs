using CalamityInheritance.Content.BaseClass.Projectiles;
using LAP.Assets.TextureRegister;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Typeless.General
{
    public class ElectricSpark : CITypelessProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 30;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }
        public override void AI()
        {
            if (Projectile.velocity.X != Projectile.velocity.X)
                Projectile.velocity.X *= -0.1f;
            if (Projectile.velocity.Y != Projectile.velocity.Y && Projectile.velocity.Y > 1f)
                Projectile.velocity.Y *= -0.5f;

            Projectile.ai[0]++;
            if (Projectile.ai[0] > 5f)
            {
                Projectile.ai[0] = 5f;
                if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
                {
                    Projectile.velocity.X *= 0.97f;
                    if (MathF.Abs(Projectile.velocity.X) < 0.01f)
                    {
                        Projectile.velocity.X = 0f;
                        Projectile.netUpdate = true;
                    }
                }
                Projectile.velocity.Y += 0.2f;
            }
            Dust spark = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.UnusedWhiteBluePurple, Alpha: 100);
            spark.position.X -= 2f;
            spark.position.Y += 2f;
            spark.scale += Main.rand.NextFloat(0f, 0.5f);
            spark.noGravity = true;
            spark.velocity.Y -= 2f;
            if (Main.rand.NextBool())
            {
                spark = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.UnusedWhiteBluePurple, Alpha: 100);
                spark.position.X -= 2f;
                spark.position.Y += 2f;
                spark.scale += Main.rand.NextFloat(0.3f, 0.8f);
                spark.noGravity = true;
                spark.velocity *= 0.1f;
            }

            if (Projectile.velocity.Y > 16f)
                Projectile.velocity.Y = 16f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) => false;
    }
}
