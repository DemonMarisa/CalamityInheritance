using CalamityInheritance.Content.BaseClass.Buff;
using CalamityInheritance.Content.Projectiles.Legendary;

namespace CalamityInheritance.Content.Buff.SummonBuff.Weapons
{
    public class ColdDivinityLegendBuff : CISummonBuff
    {
        public override int ProjectileType => ProjectileType<ColdDivinityLegendProj>();
    }
}
