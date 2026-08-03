using CalamityInheritance.Content.BaseClass.Buff;
using CalamityInheritance.Content.Projectiles.DraedonsArsenal.Summon.Header;

namespace CalamityInheritance.Content.Buff.SummonBuff.Weapons
{
    public class MountedScannerLegacyBuff : CISummonBuff
    {
        public override int ProjectileType => ProjectileType<MountedScannerSummonLegacy>();
    }
}
