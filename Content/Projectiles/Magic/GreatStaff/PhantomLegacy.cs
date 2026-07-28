using CalamityInheritance.Content.BaseClass.Projectiles;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic.GreatStaff
{
    public class PhantomLegacy : CIMagicProj
    {
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

            LAPUtilities.HomeInNPC(Projectile, 1500f, 12f, 20f, null, !Projectile.tileCollide);
        }
    }
}
