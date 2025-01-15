using CalamityMod;
using System;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class GalaxyStarold : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Mods.CalamityInheritance.Content.Content.Projectiles";
        public bool madeCoolMagicSound = false;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 160;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1f, 1f, 1f);
            if (!madeCoolMagicSound)
            {
                SoundEngine.PlaySound(SoundID.Item9, Projectile.position); // Starfury sound
                madeCoolMagicSound = true;
            }
            Projectile.ai[0]++;
            if (Projectile.ai[0] % 5 == 0)
            {
                for (int i = 0; i < Main.rand.Next(2, 4); i++) //2-3 stars
                {
                    Vector2 randVector = Vector2.One.RotatedByRandom(Math.PI * 2.0) * 0.7f;
                    Dust.NewDust(Projectile.Center, 4, 4, DustID.Enchanted_Pink, randVector.X, randVector.Y, 0, default, 1f);
                }
            }
            Projectile.rotation += Projectile.velocity.Length() / 19f;
            CalamityUtils.HomeInOnNPC(Projectile, true, 400f, 14f, 0f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 2);
            return false;
        }
    }
}
