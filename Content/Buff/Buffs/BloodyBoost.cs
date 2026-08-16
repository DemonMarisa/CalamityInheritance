using CalamityInheritance.Content.BaseClass.Buff;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Buff.Buffs
{
    public class BloodyBoost : CIBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.LAP().healingPotionMult += 0.5f;
            player.GetDamage<GenericDamageClass>() += 0.05f;
            player.statDefense += 20;
            player.endurance += 0.1f;
            player.longInvince = true;
            player.crimsonRegen = true;
        }
    }
}
