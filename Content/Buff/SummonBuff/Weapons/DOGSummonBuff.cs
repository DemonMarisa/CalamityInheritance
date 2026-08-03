using CalamityInheritance.Content.BaseClass.Buff;
using CalamityInheritance.Content.Projectiles.Summon.Normal.Worm;

namespace CalamityInheritance.Content.Buff.SummonBuff.Weapons
{
    public class DOGSummonBuff : CISummonBuff
    {
        public override int ProjectileType => ProjectileType<DOGworm>();
    }
}
