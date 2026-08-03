using CalamityInheritance.Content.Buff.DamageBuffs;
using LAP.Core.Utilities;
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
            Player.LAP().WingTimeMaxMult += 0.25f;
            Player.statDefense += 16;
            Player.wingAccRunSpeed += 0.1f;
            Player.accRunSpeed += 0.1f;
        }
    }
}
