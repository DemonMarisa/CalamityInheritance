using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Buffs.StatDebuffs
{
    public abstract class GenericBuffDefualt: ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
            base.SetStaticDefaults();    
        }
    }
}