using CalamityInheritance.Content.BaseClass.Buff;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Buff.Buffs
{
    public class TrinketofChiLegacyBuff : CIBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.AddDR(0.5f);
            player.GetDamage<GenericDamageClass>() += 0.15f;
        }
    }
}