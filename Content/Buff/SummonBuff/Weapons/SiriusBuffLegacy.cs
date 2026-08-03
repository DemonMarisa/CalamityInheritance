using CalamityInheritance.Content.BaseClass.Buff;
using CalamityInheritance.Content.Projectiles.Summon.Normal.Limits;
using LAP.Core.BaseClass;

namespace CalamityInheritance.Content.Buff.SummonBuff.Weapons
{
    public class SiriusBuffLegacy : CISummonBuff
    {
        public override int ProjectileType => ProjectileType<SiriusMinionLegacy>();
    }
}
