using CalamityInheritance.Content.BaseClass.Buff;
using CalamityInheritance.Content.Projectiles.Typeless.Accessories;

namespace CalamityInheritance.Content.Buff.SummonBuff.Accessories
{
    public class FungalClumpLegacyBuff : CISummonBuff
    {
        public override int ProjectileType => ProjectileType<FungalClumpMinion>();
    }
}