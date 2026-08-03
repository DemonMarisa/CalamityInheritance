using CalamityInheritance.Content.BaseClass.Buff;
using LAP.Core.BaseClass;

namespace CalamityInheritance.Content.Buff.SummonBuff.Weapons
{
    public class SolarGodSpiritBuff : CISummonBuff
    {
        public override int ProjectileType => ProjectileType<Projectiles.Summon.Normal.Limits.SolarGodSpiritMinionLegacy>();
    }
}
