using CalamityInheritance.Content.BaseClass.Buff;
using CalamityInheritance.Content.Projectiles.DraedonsArsenal.Summon.Header;
using LAP.Core.BaseClass;

namespace CalamityInheritance.Content.Buff.SummonBuff.Weapons
{
    public class StarSwallowerBuff : CISummonBuff
    {
        public override int ProjectileType => ProjectileType<StarSwallowerSummon>();
    }
}
