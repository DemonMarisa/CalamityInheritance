using CalamityInheritance.Content.BaseClass.Buff;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Buff.Buffs.PotionBuff
{
    public class TitanScale : CIBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.AddDR(0.05f);
            player.statDefense += 5;
            player.kbBuff = true;
            if (player.CI().BuffStatsTitanScaleTrueMelee > 0)
            {
                player.statDefense += 20;
                player.AddDR(0.05f);
            }
        }
    }
}
