using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Buffs.Potions
{
    public class ArmorShattering : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //给碎甲debuff
            player.CIMod().BuffStatsArmorShatter = true;
            player.GetDamage<RogueDamageClass>() += 0.08f;
            player.GetDamage<MeleeDamageClass>() += 0.08f;
            player.GetCritChance<RogueDamageClass>() += 8;
            player.GetCritChance<MeleeDamageClass>() += 8;
        }
    }
}
