using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Buffs.Potions
{
    public class Invincible : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.CalamityInheritance().InvincibleJam = true;
        }
    }
}
