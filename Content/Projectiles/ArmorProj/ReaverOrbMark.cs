using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;

namespace CalamityInheritance.Content.Projectiles.ArmorProj
{
    public class ReaverOrbMark : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.ArmorProj";
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            NPC target = Projectile.Center.ClosestNPCAt(1500);
            if (!Main.zenithWorld)
                CalamityUtils.MagnetSphereHitscan(Projectile, Vector2.Distance(Projectile.Center, target.Center), 8f, 0, 5, ModContent.ProjectileType<ReaverBeam>(), 1D, false);
            else
            {
                for (int i = 0; i < 4 ; i++)
                    CalamityUtils.MagnetSphereHitscan(Projectile, Vector2.Distance(Projectile.Center, target.Center), 16f, 1f, 10, ModContent.ProjectileType<ReaverBeam>(), 1D, false);
            }
        }
    }
}
