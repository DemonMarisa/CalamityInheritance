using CalamityInheritance.Content.Buff.DamageBuffs;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public bool HolyWrath;
        public bool DraconicSurge;
        public void PotionOnHit(NPC target)
        {
            if (HolyWrath)
                target.AddBuff(BuffType<CIHolyFlames>(), 300);
            if (DraconicSurge)
                target.AddBuff(BuffType<CIDragonfire>(), 300);
        }
        public void PotionBuff()
        {
        }
    }
}
