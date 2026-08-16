using CalamityInheritance.Content.BaseClass.Buff;
using CalamityInheritance.Content.Projectiles.Typeless.Accessories;

namespace CalamityInheritance.Content.Buff.SummonBuff.Accessories
{
    public class GladiatorsLocketLegacyBuff : CISummonBuff
    {
        public override int ProjectileType => ProjectileType<ShrineMarbleSword>();
    }
}