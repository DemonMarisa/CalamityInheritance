using CalamityInheritance.Content.Projectiles.DraedonsArsenal;
using CalamityInheritance.Content.Projectiles.Summon.Limits;
using LAP.Core.BaseClass;

namespace CalamityInheritance.Buffs.Summon
{
    public class StarSwallowerBuff : BaseSummonBuff
    {
        public override int ProjectileType => ProjectileType<StarSwallowerSummon>();
    }
}
