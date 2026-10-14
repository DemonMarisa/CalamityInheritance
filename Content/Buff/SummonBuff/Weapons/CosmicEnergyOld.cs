using CalamityInheritance.Content.BaseClass.Buff;
using CalamityInheritance.Content.Projectiles.ExoWeapons.CosmicSpiral;
using CalamityInheritance.Content.Projectiles.Summon.Normal.LongRange;

namespace CalamityInheritance.Content.Buff.SummonBuff.Weapons
{
    public class CosmicEnergyOld : CISummonBuff
    {
        public override int ProjectileType => ProjectileType<CosmicEnergySpiralOld>();
    }
}
