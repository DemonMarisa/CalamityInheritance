using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Core.Utils;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;

namespace CalamityInheritance.Content.Projectiles.Rogue.Explosion
{
    public class ExorcismShockwave : CIRogueProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public static float Radius = 100;
        public static float RadiusStealth = 200;
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = RogueDamage.Instance;
            Projectile.width = (int)(Projectile.CI().Stealth ? RadiusStealth * 2 : Radius * 2);
            Projectile.height = (int)(Projectile.CI().Stealth ? RadiusStealth * 2 : Radius * 2);
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 9;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            if (Projectile.timeLeft >= 5)
            {
                Projectile.alpha = (int)((1 - Projectile.ai[0]) * 255f);

                // Blast wave should be brighter on stealth strikes
                int numDust = (int)(40 * Projectile.ai[0]) + 10;

                for (int i = 0; i < numDust; i++)
                {
                    // Dust Type
                    int dustToUse = Main.rand.Next(0, 3);
                    int dustType = 0;
                    switch (dustToUse)
                    {
                        case 0:
                            dustType = 175;
                            break;
                        case 1:
                            dustType = 229;
                            break;
                        case 2:
                            dustType = 263;
                            break;
                    }

                    // Shockwave
                    Vector2 circleVelocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1));
                    circleVelocity.Normalize();
                    circleVelocity *= (Projectile.CI().Stealth ? RadiusStealth : Radius) / 10f;

                    int circle = Dust.NewDust(Projectile.Center, 1, 1, dustType, circleVelocity.X, circleVelocity.Y, 0, default, 1.5f);
                    Main.dust[circle].noGravity = true;
                }

                // Cross
                Vector2 dustLeft = new Vector2(-1, 0).RotatedBy(Projectile.rotation);
                Vector2 dustRight = new Vector2(1, 0).RotatedBy(Projectile.rotation);
                Vector2 dustUp = new Vector2(0, -1).RotatedBy(Projectile.rotation);
                Vector2 dustDown = (new Vector2(0, 1) * 2f).RotatedBy(Projectile.rotation);

                int dustScale = 7;
                Vector2 dustPos = Projectile.Center - new Vector2((dustScale - 1) * 0.5f, (dustScale - 1) * 0.5f);

                float minSpeed = 0f;
                float maxSpeed = 5f;
                float minScale = 1.9f;
                float maxScale = 2.1f;
                int dustCount = (int)(5 * Projectile.ai[0]);

                for (int i = 0; i < dustCount; i++)
                {
                    // Dust Type
                    int dustToUse = Main.rand.Next(0, 3);
                    int dustType = 0;
                    switch (dustToUse)
                    {
                        case 0:
                            dustType = 175;
                            break;
                        case 1:
                            dustType = 229;
                            break;
                        case 2:
                            dustType = 263;
                            break;
                    }

                    int left = Dust.NewDust(dustPos, dustScale, dustScale, dustType, 0f, 0f);
                    Main.dust[left].noGravity = true;
                    Main.dust[left].velocity = dustLeft * Main.rand.NextFloat(minSpeed, maxSpeed);
                    Main.dust[left].scale = Main.rand.NextFloat(minScale, maxScale);

                    int right = Dust.NewDust(dustPos, dustScale, dustScale, dustType, 0f, 0f);
                    Main.dust[right].noGravity = true;
                    Main.dust[right].velocity = dustRight * Main.rand.NextFloat(minSpeed, maxSpeed);
                    Main.dust[right].scale = Main.rand.NextFloat(minScale, maxScale);

                    int up = Dust.NewDust(dustPos, dustScale, dustScale, dustType, 0f, 0f);
                    Main.dust[up].noGravity = true;
                    Main.dust[up].velocity = dustUp * Main.rand.NextFloat(minSpeed, maxSpeed);
                    Main.dust[up].scale = Main.rand.NextFloat(minScale, maxScale);

                    int down = Dust.NewDust(dustPos, dustScale, dustScale, dustType, 0f, 0f);
                    Main.dust[down].noGravity = true;
                    Main.dust[down].velocity = dustDown * Main.rand.NextFloat(minSpeed, maxSpeed);
                    Main.dust[down].scale = Main.rand.NextFloat(minScale, maxScale);
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => LAPUtilities.CircularHitboxCollision(Projectile.Center, Projectile.CI().Stealth ? RadiusStealth : Radius, targetHitbox);
    }
}
