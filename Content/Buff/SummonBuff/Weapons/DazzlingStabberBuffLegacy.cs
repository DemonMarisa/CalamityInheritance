using CalamityInheritance.Content.BaseClass.Buff;
using CalamityInheritance.Content.Projectiles.Summon.Normal.LongRange;
using LAP.Core.BaseClass;

namespace CalamityInheritance.Content.Buff.SummonBuff.Weapons
{
    public class DazzlingStabberBuffLegacy : CISummonBuff
    {
        public override int ProjectileType => ProjectileType<DazzlingStabberProj>();
    }
}
