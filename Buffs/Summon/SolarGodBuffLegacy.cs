using CalamityInheritance.Content.Projectiles.Summon.Limits;
using LAP.Core.BaseClass;

namespace CalamityInheritance.Buffs.Summon
{
    public class SolarGodBuffLegacy : BaseSummonBuff
    {
        public override int ProjectileType => ProjectileType<SolarGodLegacy>();
    }
}
