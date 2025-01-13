using CalamityMod.CalPlayer;
using CalamityInheritance;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Projectiles.Magic;
using CalamityInheritance.Content.Projectiles.Magic.Ray;

namespace CalamityInheritance.Content.Projectiles.ArmorProj
{
    public class ReaverOrbMark : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Magic";
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
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            NPC target = Projectile.Center.ClosestNPCAt(1500);
            CalamityUtils.MagnetSphereHitscan(Projectile, Vector2.Distance(Projectile.Center, target.Center), 8f, 0, 5, ModContent.ProjectileType<ReaverBeam>(), 1D, false);
        }
    }
}
