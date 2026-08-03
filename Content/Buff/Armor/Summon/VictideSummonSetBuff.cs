using CalamityInheritance.Content.BaseClass.Buff;
using CalamityInheritance.Content.Projectiles.Armor.Summon.Header;

namespace CalamityInheritance.Content.Buff.Armor.Summon
{
    public class VictideSummonSetBuff : CISummonBuff
    {
        public override int ProjectileType => ProjectileType<VictideUrchin>();
    }
}
