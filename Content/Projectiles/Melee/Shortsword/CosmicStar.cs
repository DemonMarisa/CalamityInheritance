using CalamityInheritance.Content.BaseClass.Projectiles;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Shortsword
{
    public class CosmicStar : CIMeleeProj
    {
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
            Projectile.DamageType = DamageClass.Melee;
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
            LAPUtilities.HomeInNPC(Projectile, 400f, 14f, 0f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 2);
            return false;
        }
    }
}
