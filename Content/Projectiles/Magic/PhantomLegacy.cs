
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class PhantomLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override void SetStaticDefaults() => ProjectileID.Sets.CultistIsResistantTo[Type] = true;
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = 2;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            Projectile.rotation += 0.01f;

            Lighting.AddLight(Projectile.Center, 0.2f, 0.2f, 0.2f);

            for (int i = 0; i < 2; i++)
            {
                int spectre = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SpectreStaff, 0f, 0f, 100, default, 1f);
                Main.dust[spectre].noGravity = true;
            }

            CalamityUtils.HomeInOnNPC(Projectile, !Projectile.tileCollide, 1500f, 12f, 20f);
        }
    }
}
