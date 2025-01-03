using CalamityMod.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray
{
    public class BloodOrb : ModProjectile
    {
        public override string Texture => "CalamityInheritance/Content/Projectiles/InvisibleProj";
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            CalamityUtils.MagnetSphereHitscan(Projectile, 300f, 6f, 0f, 5, ModContent.ProjectileType<BloodBolt>());
        }
    }
}
