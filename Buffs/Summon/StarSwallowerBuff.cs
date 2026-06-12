using CalamityInheritance.Content.Projectiles.DraedonsArsenal;
using LAP.Core.BaseClass;

namespace CalamityInheritance.Buffs.Summon
{
    public class StarSwallowerBuff : BaseSummonBuff
    {
        public override int ProjectileType => ProjectileType<StarSwallowerSummon>();
    }
}
