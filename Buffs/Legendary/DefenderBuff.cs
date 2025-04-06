using CalamityInheritance.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Buffs.Legendary
{
    public class DefenderBuff: ModBuff    
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.CIMod().DefenderPower = true;
        }
    }
}