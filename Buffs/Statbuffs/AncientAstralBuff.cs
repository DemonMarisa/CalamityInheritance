using CalamityInheritance.Utilities;
using CalamityMod;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Buffs.Statbuffs
{
    public class AncientAstralBuff: ModBuff, ILocalizedModType
    {
        public new string LocalizationCategory => "Buffs";
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            var calPlayer = player.Calamity();    
            int getDef = player.statDefense;
            int defenseBuff = (int)(getDef * 0.30f);
            player.statDefense += defenseBuff;
            player.endurance += 0.3f;
            calPlayer.defenseDamageRatio *= 0.5f;
        }

    }

}