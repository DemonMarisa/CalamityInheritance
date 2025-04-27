using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityMod;

namespace CalamityInheritance.Buffs.Potions
{
    public class PenumbraBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            var calPlayer = player.Calamity();  
            calPlayer.stealthGenStandstill += 0.15f;
            calPlayer.stealthGenMoving += 0.1f;
        }
    }
}
