using CalamityInheritance.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Buffs.Statbuffs
{
    public class BloodyBoost : ModBuff, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.CalamityInheritance().BuffStatBloodPact = true;
        }
    }
}
