using CalamityInheritance.Core.Path;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass.Projectiles
{
    public abstract class CIMeleeRogueProj : ModProjectile, ILocalizedModType
    {
        public override string LocalizationCategory => LocalizationPath.RogueMeleeProj;
        public Player Owner => Main.player[Projectile.owner];
    }
}
