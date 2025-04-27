using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using Terraria.ID;

namespace CalamityInheritance.Buffs.Potions
{
    public class CadancesGrace : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //这条只用于在lifeMax那里操作生命上限的统一加成，别删了
            player.CIMod().BuffStatsCadence = true;
            player.lifeMagnet = true;
            player.lifeRegen += 10;
            if (Main.zenithWorld)
                player.AddBuff(BuffID.Lovestruck, 36000);
        }
    }
}
