using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using LAP.Assets.TextureRegister;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class TerraFireGreenLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";

        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 150;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * 0.45f / 255f, (255 - Projectile.alpha) * 0.05f / 255f);
            if (Projectile.timeLeft > 150)
            {
                Projectile.timeLeft = 150;
            }
            if (Projectile.ai[0] > 7f)
            {
                if (Main.zenithWorld)
                    CIFunction.HomeInOnNPC(Projectile, true, 1800f, 24f, 20f);
                float pScale = 1f;
                if (Projectile.ai[0] == 8f)
                {
                    pScale = 0.25f;
                }
                else if (Projectile.ai[0] == 9f)
                {
                    pScale = 0.5f;
                }
                else if (Projectile.ai[0] == 10f)
                {
                    pScale = 0.75f;
                }
                if (Projectile.ai[0] > 15f)
                Projectile.ai[0] += 1f;
                int dType = 107;
                if (Main.rand.NextBool(2))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dType, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 0.75f);
                        Dust dust = Main.dust[d];
                        if (Main.rand.NextBool(3))
                        {
                            dust.noGravity = true;
                            dust.scale *= 2f;
                            dust.velocity.X *= 2f;
                            dust.velocity.Y *= 2f;
                        }
                        else
                        {
                            dust.scale *= 0.75f;
                        }
                        dust.velocity.X *= 1.2f;
                        dust.velocity.Y *= 1.2f;
                        dust.scale *= pScale;
                        dust.velocity += Projectile.velocity;
                        if (!dust.noGravity)
                        {
                            dust.velocity *= 0.5f;
                        }
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
            target.immune[Projectile.owner] = 5;
            target.AddBuff(BuffID.CursedInferno, 90);
        }
    }
}
