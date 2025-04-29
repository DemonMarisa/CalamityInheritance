using CalamityInheritance.Utilities;
using CalamityMod.Projectiles.Ranged;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles
{
    public class CalProjOverride : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Projectile proj)
        {
            if (proj.type == ModContent.ProjectileType<StarmageddonStar>())
                proj.CalamityInheritance().PingAsSplit = true;
            if (proj.type == ModContent.ProjectileType<StarmageddonStar2>())
                proj.CalamityInheritance().PingAsSplit = true;
            if (proj.type == ModContent.ProjectileType<StarmageddonBinaryStarCenter>())
                proj.CalamityInheritance().PingAsSplit = true;
        }
    }
}
