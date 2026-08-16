using CalamityInheritance.Core.Path;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass.Projectiles
{
    public abstract class CIArmorProj : ModProjectile, ILocalizedModType
    {
        public override string LocalizationCategory => LocalizationPath.ArmorProjectile;
        public Player Owner => Main.player[Projectile.owner];
    }
}
