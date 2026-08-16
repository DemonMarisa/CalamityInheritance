using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (ContactDamageReduction != 1)
            {
                modifiers.FinalDamage *= ContactDamageReduction;
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            modifiers.ModifyHurtInfo += ModifyHurtInfo_Shield;
            float damageMultAdd = 1f;
            ModifyHurt_Accessories(ref modifiers, ref damageMultAdd);
            if (damageMultAdd != 1f)
                modifiers.SourceDamage *= damageMultAdd;
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            ArmorOnHurt(info);
            MainOnHurt(info);
            OnHurt_Accessories(info);
        }
    }
}
