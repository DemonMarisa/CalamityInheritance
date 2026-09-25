using CalamityInheritance.Content.Buff.DamageBuffs;
using LAP.Core.GlobalInstance.Players;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public bool HolyWrath;
        public bool DraconicSurge;
        public bool Triumph;
        public int BuffStatsTitanScaleTrueMelee;
        public void PotionOnHit(NPC target)
        {
            if (HolyWrath)
                target.AddBuff(BuffType<CIHolyFlames>(), 300);
            if (DraconicSurge)
                target.AddBuff(BuffType<CIDragonfire>(), 300);
            BuffStatsTitanScaleTrueMelee = 600;
        }
        public void PotionHurt(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (Triumph)
            {
                float reduce = 0.15f * (1f - (npc.life / (float)npc.lifeMax));
                ContactDamageReduction *= (1f - reduce);
            }
        }
        public void PotionBuffReset()
        {
            if (BuffStatsTitanScaleTrueMelee > 0)
                BuffStatsTitanScaleTrueMelee--;
            Triumph = false;
            DraconicSurge = false;
            HolyWrath = false;
        }
        public void PotionBuff()
        {
        }
    }
}
