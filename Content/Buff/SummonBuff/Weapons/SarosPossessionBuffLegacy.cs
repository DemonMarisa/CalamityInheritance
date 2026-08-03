using CalamityInheritance.Content.BaseClass.Buff;
using CalamityInheritance.Content.Projectiles.Summon.Normal.Limits;

namespace CalamityInheritance.Content.Buff.SummonBuff.Weapons
{
    public class SarosPossessionBuffLegacy : CISummonBuff
    {
        public override int ProjectileType => ProjectileType<SarosAuraLegacy>();
    }
}
