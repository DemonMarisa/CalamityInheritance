using CalamityInheritance.Content.BaseClass.Buff;
using CalamityInheritance.Content.Projectiles.Summon.Normal.CloseRange;
using LAP.Core.BaseClass;

namespace CalamityInheritance.Content.Buff.SummonBuff.Weapons
{
    public class AncientClasperBuff : CISummonBuff
    {
        public override int ProjectileType => ProjectileType<AncientClasper>();
    }
}
